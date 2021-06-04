using System.Collections.Generic;
using hefesto.admin.Models;
using hefesto.base_hefesto.Pagination;
using System.Threading.Tasks;

namespace hefesto.admin.Services
{
    public interface IAdmPageService
    {
        void SetTransient(List<AdmPage> list);
        void SetTransient(AdmPage item);

        Task<BasePaged<AdmPage>> GetPage(string route, PaginationFilter filter);
    }
}