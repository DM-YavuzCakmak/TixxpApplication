using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using Tixxp.Business.Services.Abstract.PersonnelRoleService;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Business.Services.Abstract.RoleService;
using Tixxp.Core.Utilities.Results.Concrete;
using Tixxp.Entities.Personnel;
using Tixxp.Entities.PersonnelRole;

namespace Tixxp.WebApp.Controllers
{
    public class PersonnelController : Controller
    {
        private readonly IPersonnelService _personnelService;
        private readonly IPersonnelRoleService _personnelRoleService;
        private readonly IRoleService _roleService;
        private readonly IStringLocalizer<PersonnelController> _localizer;


        public PersonnelController(
            IPersonnelService personnelService,
            IPersonnelRoleService personnelRoleService,
            IRoleService roleService,
            IStringLocalizer<PersonnelController> localizer)
        {
            _personnelService = personnelService;
            _personnelRoleService = personnelRoleService;
            _roleService = roleService;
            _localizer = localizer;
        }

        // ----------------------------------------------------
        // INDEX
        // ----------------------------------------------------
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Auth");

            long currentUserId = Convert.ToInt64(userId);
            var currentUser = _personnelService.GetById(currentUserId);

            if (!currentUser.Success || currentUser.Data == null)
                return RedirectToAction("Login", "Auth");

            string companyIdentifier = currentUser.Data.CompanyIdentifier;

            var listResult = _personnelService.GetList(x =>
                x.CompanyIdentifier == companyIdentifier &&
                x.Id != currentUserId &&
                !x.IsDeleted);

            return View(listResult?.Data?.ToList() ?? new List<PersonnelEntity>());
        }

        // ----------------------------------------------------
        // HIERARCHY TREE
        // ----------------------------------------------------
        [HttpGet]
        public JsonResult GetHierarchyTree()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Json(new ErrorResult(_localizer["personnel.ERROR.NOT_FOUND"]));

            long currentUserId = Convert.ToInt64(userId);
            var currentUser = _personnelService.GetById(currentUserId);

            if (!currentUser.Success || currentUser.Data == null)
                return Json(new ErrorResult(_localizer["personnel.ERROR.NOT_FOUND"]));

            var list = _personnelService.GetList(x =>
                x.CompanyIdentifier == currentUser.Data.CompanyIdentifier &&
                !x.IsDeleted);

            if (!list.Success || list.Data == null)
                return Json(new ErrorResult(_localizer["personnel.ERROR.NOT_FOUND"]));

            var tree = BuildTree(null, list.Data);

            return Json(new SuccessDataResult<object>(tree));
        }

        private List<object> BuildTree(long? parentId, IEnumerable<PersonnelEntity> all)
        {
            return all
                .Where(x => x.ParentId == parentId)
                .Select(x => new
                {
                    id = x.Id,
                    name = $"{x.FirstName} {x.LastName}",
                    children = BuildTree(x.Id, all)
                }).ToList<object>();
        }

        // ----------------------------------------------------
        // GET BY ID
        // ----------------------------------------------------
        [HttpGet]
        public JsonResult GetById(long id)
        {
            var result = _personnelService.GetById(id);

            if (!result.Success || result.Data == null)
                return Json(new ErrorResult(_localizer["personnel.ERROR.NOT_FOUND"]));

            var p = result.Data;

            return Json(new SuccessDataResult<object>(new
            {
                id = p.Id,
                firstName = p.FirstName,
                lastName = p.LastName,
                email = p.Email,
                phone = p.Phone
            }));
        }

        // ----------------------------------------------------
        // SAVE (ADD / UPDATE)
        // ----------------------------------------------------
        [HttpPost]
        public JsonResult Save(PersonnelEntity model)
        {
            // VALIDATION - BASIC CHECKS
            if (model == null)
                return Json(new ErrorResult(_localizer["personnel.ERROR.MODEL_INVALID"]));

            if (string.IsNullOrWhiteSpace(model.FirstName) ||
                string.IsNullOrWhiteSpace(model.LastName) ||
                string.IsNullOrWhiteSpace(model.Email))
            {
                return Json(new ErrorResult(_localizer["personnel.ERROR.MODEL_INVALID"]));
            }

            // EMAIL VALIDATION
            if (!System.Text.RegularExpressions.Regex.IsMatch(model.Email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$"))
                return Json(new ErrorResult(_localizer["personnel.ERROR.EMAIL_INVALID"]));

            // PHONE VALIDATION (OPTIONAL)
            if (!string.IsNullOrEmpty(model.Phone))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(model.Phone, @"^[0-9]{10}$"))
                    return Json(new ErrorResult(_localizer["personnel.ERROR.PHONE_INVALID"]));
            }

            // AUTH
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Json(new ErrorResult(_localizer["personnel.ERROR.NOT_FOUND"]));

            long currentUserId = Convert.ToInt64(userId);
            var currentUser = _personnelService.GetById(currentUserId);

            if (!currentUser.Success || currentUser.Data == null)
                return Json(new ErrorResult(_localizer["personnel.ERROR.NOT_FOUND"]));

            // ----------------------------------------------------
            // CREATE NEW USER
            // ----------------------------------------------------
            if (model.Id == 0)
            {
                if (string.IsNullOrWhiteSpace(model.Password))
                    return Json(new ErrorResult(_localizer["personnel.ERROR.PASSWORD_REQUIRED"]));

                byte[] salt = _personnelService.GenerateSalt();
                string saltStr = Convert.ToBase64String(salt);
                string passwordHash = _personnelService.GenerateSha256Hash(salt, model.Password);

                model.UserName = model.Email;
                model.Password = passwordHash;
                model.Salt = saltStr;
                model.IsActive = true;
                model.IsDeleted = false;
                model.LoginTypeId = 2;
                model.Created_Date = DateTime.Now;
                model.CreatedBy = currentUserId;
                model.CompanyIdentifier = currentUser.Data.CompanyIdentifier;
                model.NationalIdNumber ??= "123";

                var addResult = _personnelService.Add(model);

                if (!addResult.Success)
                    return Json(new ErrorResult(_localizer["personnel.ERROR.SAVE_FAILED"]));

                return Json(new SuccessResult(_localizer["personnel.SUCCESS.SAVED"]));
            }

            // ----------------------------------------------------
            // UPDATE EXISTING USER
            // ----------------------------------------------------
            var existing = _personnelService.GetById(model.Id);
            if (!existing.Success || existing.Data == null)
                return Json(new ErrorResult(_localizer["personnel.ERROR.NOT_FOUND"]));

            var p = existing.Data;

            p.FirstName = model.FirstName;
            p.LastName = model.LastName;
            p.Email = model.Email;
            p.UserName = model.Email;
            p.Phone = model.Phone;
            p.Updated_Date = DateTime.Now;
            p.UpdatedBy = currentUserId;

            var updateResult = _personnelService.Update(p);

            if (!updateResult.Success)
                return Json(new ErrorResult(_localizer["personnel.ERROR.SAVE_FAILED"]));

            return Json(new SuccessResult(_localizer["personnel.SUCCESS.SAVED"]));
        }

        // ----------------------------------------------------
        // DELETE
        // ----------------------------------------------------
        [HttpPost]
        public JsonResult Delete(long id)
        {
            var user = _personnelService.GetById(id);

            if (!user.Success || user.Data == null)
                return Json(new ErrorResult(_localizer["personnel.ERROR.NOT_FOUND"]));

            var p = user.Data;
            p.IsDeleted = true;
            p.Updated_Date = DateTime.Now;

            var update = _personnelService.Update(p);

            if (!update.Success)
                return Json(new ErrorResult(_localizer["personnel.ERROR.DELETE_FAILED"]));

            return Json(new SuccessResult(_localizer["personnel.SUCCESS.DELETED"]));
        }

        // ----------------------------------------------------
        // GET ALL ROLES
        // ----------------------------------------------------
        [HttpGet]
        public JsonResult GetAllRole()
        {
            var result = _roleService.GetAll();

            if (!result.Success || result.Data == null)
                return Json(new ErrorResult(_localizer["personnel.ERROR.ROLE_NOT_FOUND"]));

            var roles = result.Data.Select(r => new { id = r.Id, name = r.Name }).ToList();

            return Json(new SuccessDataResult<object>(roles));
        }

        // ----------------------------------------------------
        // ASSIGN ROLE
        // ----------------------------------------------------
        [HttpPost]
        public JsonResult AssignRole(long personnelId, long roleId)
        {
            if (personnelId <= 0 || roleId <= 0)
                return Json(new ErrorResult(_localizer["personnel.ERROR.INVALID_ID"]));

            var exists = _personnelRoleService
                .GetFirstOrDefault(x => x.PersonnelId == personnelId && x.RoleId == roleId);

            if (exists.Success && exists.Data != null)
                return Json(new ErrorResult(_localizer["personnel.ERROR.ROLE_ALREADY_ASSIGNED"]));

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Json(new ErrorResult(_localizer["personnel.ERROR.NOT_FOUND"]));

            var newRole = new PersonnelRoleEntity
            {
                PersonnelId = personnelId,
                RoleId = roleId,
                Created_Date = DateTime.Now,
                CreatedBy = Convert.ToInt64(userId)
            };

            var addResult = _personnelRoleService.Add(newRole);

            if (!addResult.Success)
                return Json(new ErrorResult(_localizer["personnel.ERROR.ROLE_ASSIGN_FAILED"]));

            return Json(new SuccessResult(_localizer["personnel.SUCCESS.ROLE_ASSIGNED"]));
        }

        // ----------------------------------------------------
        // GET ROLES OF PERSONNEL
        // ----------------------------------------------------
        [HttpGet]
        public JsonResult GetRolesByPersonnelId(long personnelId)
        {
            var result = _personnelRoleService.GetList(x =>
                x.PersonnelId == personnelId &&
                !x.IsDeleted);

            if (!result.Success || result.Data == null)
                return Json(new ErrorResult(_localizer["personnel.ERROR.ROLE_NOT_FOUND"]));

            var roleList = new List<object>();

            foreach (var pr in result.Data)
            {
                var role = _roleService.GetById(pr.RoleId);
                if (role.Success && role.Data != null)
                {
                    roleList.Add(new
                    {
                        id = role.Data.Id,
                        name = role.Data.Name
                    });
                }
            }

            return Json(new SuccessDataResult<object>(roleList));
        }
    }
}
