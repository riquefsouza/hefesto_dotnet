using hefesto.admin.Models;
using hefesto.base_hefesto.Pagination;
using System.Threading.Tasks;

namespace hefesto.admin.Services
{
    public interface IAdmParameterCategoryService
    {
        Task<BasePaged<AdmParameterCategory>> GetPage(string route, PaginationFilter filter);
    }
}