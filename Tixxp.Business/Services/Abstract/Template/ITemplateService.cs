using System.Collections.Generic;
using Tixxp.Business.DataTransferObjects.Template;
using Tixxp.Entities.Role;

namespace Tixxp.Business.Services.Abstract.Template
{
    public interface ITemplateService
    {
        // Template'e bağlı roller
        List<RoleEntity> GetRolesByTemplateId(int templateId);

        // Tüm roller (veritabanından)
        List<RoleEntity> GetAllRoles();

        // Sabit template listesini döner (Id + Name)
        List<RoleGroupTemplateDto> GetTemplateList();

        // RoleGroup adı varsa _1, _2 takısı ekler
        string GetAvailableRoleGroupName(string baseName);

        // Sabit templateId'ye karşılık gelen grup adı (örneğin: Satış Görevlisi)
        string GetGroupNameByTemplateId(int templateId);
    }
}
