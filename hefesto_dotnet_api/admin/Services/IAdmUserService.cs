using System.Collections.Generic;
using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.base_hefesto.Models;
using hefesto.base_hefesto.Pagination;

namespace hefesto.admin.Services
{
    public interface IAdmUserService
    {
        void SetTransient(List<AdmUser> list);
        void SetTransient(AdmUser item);
        Task<AdmUser> Authenticate(string login, string password);
        bool VerifyPassword(string password, string hashPassword);

        Task<BasePaged<AdmUser>> GetPage(string route, PaginationFilter filter);
    }
}