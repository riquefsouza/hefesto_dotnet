using System.Collections.Generic;
using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.base_hefesto.Models;
using hefesto.base_hefesto.Pagination;

namespace hefesto.admin.Services
{
    public interface IAdmProfileService
    {
        void SetTransient(List<AdmProfile> list);
        void SetTransient(AdmProfile item);
        Task<List<MenuItemDTO>> mountMenuItem(List<long> listaIdProfile);
        Task<List<AdmProfile>> findProfilesByPage(long pageId);
        Task<List<AdmProfile>> findProfilesByUser(long userId);

        Task<BasePaged<AdmProfile>> GetPage(string route, PaginationFilter filter);
    }
}