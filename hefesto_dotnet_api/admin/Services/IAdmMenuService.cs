using System.Collections.Generic;
using hefesto.admin.Models;
using hefesto.base_hefesto.Pagination;
using System.Threading.Tasks;
using hefesto.base_hefesto.Services;

namespace hefesto.admin.Services
{
    public interface IAdmMenuService : IBaseCrud<AdmMenu, long>
    {
        void SetTransientWithoutSubMenus(List<AdmMenu> list);
        void SetTransient(List<AdmMenu> list);
        void SetTransientSubMenus(AdmMenu item, List<AdmMenu> subMenus);
        void SetTransient(AdmMenu item);
    }
}