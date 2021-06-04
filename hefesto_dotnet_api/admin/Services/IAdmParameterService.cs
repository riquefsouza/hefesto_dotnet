using System.Collections.Generic;
using hefesto.admin.Models;
using hefesto.base_hefesto.Pagination;
using System.Threading.Tasks;

namespace hefesto.admin.Services
{
    public interface IAdmParameterService
    {
        void SetTransient(List<AdmParameter> list);
        void SetTransient(AdmParameter item);

        Task<BasePaged<AdmParameter>> GetPage(string route, PaginationFilter filter);
    }
}