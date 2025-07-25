using System.Collections.Generic;
using System.Linq;
using Tixxp.Business.DataTransferObjects.Template;
using Tixxp.Business.Services.Abstract.Template;
using Tixxp.Entities.Role;
using Tixxp.Infrastructure.DataAccess.Abstract.Role;
using Tixxp.Infrastructure.DataAccess.Abstract.RoleGroup;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.RoleGroup;

namespace Tixxp.Business.Services.Concrete.Template
{
    public class TemplateService : ITemplateService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleGroupRepository _roleGroupRepository;


        // Sabit tanımlı template'ler
        private readonly Dictionary<int, (string groupName, List<string> roleNames)> _templates = new()
        {
            { 1, ("Satış Görevlisi", new List<string> { "TIXXP_PRODUCT_SALE", "TIXXP_RESERVATION_SALE", "TIXXP_REPORT_PRODUCT_SALE" }) }
            //{ 2, ("Etkinlik Operasyon", new List<string> { "Backstage", "Barkod Okuyucu", "Sahne Erişimi" }) },
            //{ 3, ("Finans", new List<string> { "Muhasebe", "Raporlama", "Ödeme Kontrol" }) }
        };

        public TemplateService(IRoleRepository roleRepository, IRoleGroupRepository roleGroupRepository)
        {
            _roleRepository = roleRepository;
            _roleGroupRepository = roleGroupRepository;
        }

        public List<RoleEntity> GetRolesByTemplateId(int templateId)
        {
            if (!_templates.TryGetValue(templateId, out var template))
                return new List<RoleEntity>();

            var roleNames = template.roleNames;

            var roles = _roleRepository
                .GetList(r => roleNames.Contains(r.Name) && !r.IsDeleted)
                .ToList();

            return roles;
        }

        public string GetSuggestedGroupName(string baseName, List<string> existingGroupNames)
        {
            if (!existingGroupNames.Contains(baseName))
                return baseName;

            int suffix = 1;
            string newName;
            do
            {
                newName = $"{baseName}_{suffix}";
                suffix++;
            } while (existingGroupNames.Contains(newName));

            return newName;
        }

        public string GetGroupNameByTemplateId(int templateId)
        {
            return _templates.TryGetValue(templateId, out var template)
                ? template.groupName
                : "YeniRoleGroup";
        }

        public List<RoleGroupTemplateDto> GetTemplateList()
        {
            return _templates
                .Select(x => new RoleGroupTemplateDto
                {
                    Id = x.Key,
                    Name = x.Value.groupName
                }).ToList();
        }

        public List<RoleEntity> GetAllRoles()
        {
            return _roleRepository.GetList(x => !x.IsDeleted).ToList();
        }

        public string GetAvailableRoleGroupName(string baseName)
        {
            var existingNames = _roleGroupRepository
                .GetList(x => !x.IsDeleted)
                .Select(x => x.Name)
                .ToList();

            if (!existingNames.Contains(baseName))
                return baseName;

            int counter = 1;
            string candidateName;
            do
            {
                candidateName = $"{baseName} {counter}";
                counter++;
            } while (existingNames.Contains(candidateName));

            return candidateName;
        }
    }
}
