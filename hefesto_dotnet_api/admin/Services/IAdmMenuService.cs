using System.Collections.Generic;
using hefesto.admin.Models;
using hefesto.base_hefesto.Pagination;
using System.Threading.Tasks;

namespace hefesto.admin.Services
{
    public interface IAdmMenuService
    {
        void SetTransientWithoutSubMenus(List<AdmMenu> list);
        void SetTransient(List<AdmMenu> list);
        void SetTransientSubMenus(AdmMenu item, List<AdmMenu> subMenus);
        void SetTransient(AdmMenu item);

        Task<BasePaged<AdmMenu>> GetPage(string route, PaginationFilter filter);
    }
}