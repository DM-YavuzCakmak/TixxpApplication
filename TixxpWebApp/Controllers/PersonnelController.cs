// CONTROLLER
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tixxp.Business.DataTransferObjects.Personnel.Login;
using Tixxp.Business.Services.Abstract.PersonnelRoleService;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Business.Services.Abstract.RoleService;
using Tixxp.Core.Utilities.Results.Abstract;
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

        public PersonnelController(IPersonnelService personnelService, IPersonnelRoleService personnelRoleService, IRoleService roleService)
        {
            _personnelService = personnelService;
            _personnelRoleService = personnelRoleService;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            var personnelIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (personnelIdClaim == null)
                return RedirectToAction("Login", "Auth");

            long personnelId = Convert.ToInt64(personnelIdClaim.Value);

            var personnelResult = _personnelService.GetById(personnelId);
            if (!personnelResult.Success || personnelResult.Data == null)
                return RedirectToAction("Login", "Auth");

            var companyIdentifier = personnelResult.Data.CompanyIdentifier;

            var personnelListResult = _personnelService.GetList(x => x.CompanyIdentifier == companyIdentifier && x.Id != personnelId && !x.IsDeleted);
            var personnelList = personnelListResult?.Data?.ToList() ?? new List<PersonnelEntity>();

            return View(personnelList);
        }

        [HttpGet]
        public JsonResult GetHierarchyTree()
        {
            var personnelIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (personnelIdClaim == null)
                return Json(new { success = false, message = "Oturum bulunamadı" });

            long personnelId = Convert.ToInt64(personnelIdClaim.Value);
            var currentUser = _personnelService.GetById(personnelId);
            if (!currentUser.Success || currentUser.Data == null)
                return Json(new { success = false, message = "Kullanıcı bilgisi alınamadı" });

            var listResult = _personnelService.GetList(x => x.CompanyIdentifier == currentUser.Data.CompanyIdentifier && !x.IsDeleted);
            if (!listResult.Success || listResult.Data == null)
                return Json(new { success = false, message = "Veri alınamadı" });

            var allPersonnel = listResult.Data;

            var tree = BuildTree(null, allPersonnel);

            return Json(new { success = true, data = tree });
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

        [HttpGet]
        public JsonResult GetById(long id)
        {
            var result = _personnelService.GetById(id);
            if (result.Success && result.Data != null)
            {
                var personnel = result.Data;
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        id = personnel.Id,
                        firstName = personnel.FirstName,
                        lastName = personnel.LastName,
                        email = personnel.Email,
                        phone = personnel.Phone
                    }
                });
            }

            return Json(new { success = false, message = "Kullanıcı bulunamadı." });
        }


        [HttpPost]
        public JsonResult Save(PersonnelEntity model)
        {
            if (string.IsNullOrWhiteSpace(model.FirstName) ||
                string.IsNullOrWhiteSpace(model.LastName) ||
                string.IsNullOrWhiteSpace(model.Email))
            {
                return Json(new ErrorResult("Zorunlu alanlar doldurulmalıdır."));
            }

            var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdStr))
                return Json(new ErrorResult("Oturum bilgisi bulunamadı."));

            long currentUserId = Convert.ToInt64(currentUserIdStr);

            // Kullanıcının sistemde varlığını kontrol et
            var currentUserResult = _personnelService.GetById(currentUserId);
            if (!currentUserResult.Success || currentUserResult.Data == null)
                return Json(new ErrorResult("Kullanıcı bilgisi alınamadı."));

            if (model.Id == 0)
            {
                // Yeni kullanıcı ekleme
                if (string.IsNullOrWhiteSpace(model.Password))
                    return Json(new ErrorResult("Şifre alanı zorunludur."));

                var saltBytes = _personnelService.GenerateSalt();
                var saltBase64 = Convert.ToBase64String(saltBytes);
                var hash = _personnelService.GenerateSha256Hash(saltBytes, model.Password);

                model.UserName = model.Email;
                model.Password = hash;
                model.Salt = saltBase64;
                model.IsActive = true;
                model.IsDeleted = false;
                model.LoginTypeId = 2;
                model.Created_Date = DateTime.Now;
                model.CreatedBy = currentUserId;
                model.CompanyIdentifier = currentUserResult.Data.CompanyIdentifier;

                model.NationalIdNumber ??= "123";
                var addResult = _personnelService.Add(model);
                return Json(addResult);
            }
            else
            {
                var existing = _personnelService.GetById(model.Id);
                if (!existing.Success || existing.Data == null)
                    return Json(new ErrorResult("Güncellenecek kullanıcı bulunamadı."));

                var personnel = existing.Data;

                personnel.FirstName = model.FirstName;
                personnel.LastName = model.LastName;
                personnel.Email = model.Email;
                personnel.UserName = model.Email;
                personnel.Phone = model.Phone;
                personnel.Updated_Date = DateTime.Now;
                personnel.UpdatedBy = currentUserId;

                var updateResult = _personnelService.Update(personnel);
                return Json(updateResult);
            }
        }

        [HttpPost]
        public JsonResult Delete(long id)
        {
            var personnelResult = _personnelService.GetById(id);
            if (!personnelResult.Success || personnelResult.Data == null)
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });

            var personnel = personnelResult.Data;
            personnel.IsDeleted = true;
            personnel.Updated_Date = DateTime.Now;

            var updateResult = _personnelService.Update(personnel);

            if (updateResult.Success)
                return Json(new { success = true, message = "Kullanıcı başarıyla silindi." });

            return Json(new { success = false, message = "Silme işlemi sırasında hata oluştu." });
        }


        [HttpGet]
        public JsonResult GetAllRole()
        {
            var result = _roleService.GetAll();
            if (result.Success && result.Data != null)
            {
                var roles = result.Data.Select(r => new
                {
                    id = r.Id,
                    name = r.Name
                }).ToList();

                return Json(new { success = true, data = roles });
            }

            return Json(new { success = false, message = "Rol listesi alınamadı." });
        }


        [HttpPost]
        public JsonResult AssignRole(long personnelId, long roleId)
        {
            if (personnelId <= 0 || roleId <= 0)
                return Json(new ErrorResult("Geçersiz kullanıcı veya rol bilgisi."));

            var existing = _personnelRoleService.GetFirstOrDefault(x => x.PersonnelId == personnelId && x.RoleId == roleId);
            if (existing.Success && existing.Data != null)
            {
                return Json(new ErrorResult("Kullanıcı bu role sahip."));
            }

            var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdStr))
                return Json(new ErrorResult("Oturum bilgisi bulunamadı."));

            long currentUserId = Convert.ToInt64(currentUserIdStr);


            var newRole = new PersonnelRoleEntity
            {
                PersonnelId = personnelId,
                RoleId = roleId,
                Created_Date = DateTime.Now,
                CreatedBy = currentUserId
            };

            var addResult = _personnelRoleService.Add(newRole);
            return Json(addResult);
        }

        [HttpGet]
        public JsonResult GetRolesByPersonnelId(long personnelId)
        {
            var personnelRolesResult = _personnelRoleService.GetList(x => x.PersonnelId == personnelId && !x.IsDeleted);
            if (!personnelRolesResult.Success || personnelRolesResult.Data == null)
            {
                return Json(new { success = false, message = "Rol atamaları alınamadı." });
            }

            var roleList = new List<object>();
            foreach (var item in personnelRolesResult.Data)
            {
                var roleResult = _roleService.GetById(item.RoleId);
                if (roleResult.Success && roleResult.Data != null)
                {
                    roleList.Add(new
                    {
                        id = roleResult.Data.Id,
                        name = roleResult.Data.Name
                    });
                }
            }

            return Json(new { success = true, data = roleList });
        }
    }
}