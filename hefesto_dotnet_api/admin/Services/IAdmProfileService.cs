using System.Collections.Generic;
using System.Threading.Tasks;
using hefesto.admin.Models;

using hefesto.admin.VO;
using hefesto.base_hefesto.Services;

namespace hefesto.admin.Services
{
    public interface IAdmProfileService : IBaseCrud<AdmProfile, long>
    {
        void SetTransient(List<AdmProfile> list);
        void SetTransient(AdmProfile item);
        
        Task<List<AdmProfile>> FindProfilesByPage(long pageId);
        Task<List<AdmProfile>> FindProfilesByUser(long userId);
        Task<List<PermissionVO>> GetPermission(AuthenticatedUserVO authenticatedUser);
    }
}