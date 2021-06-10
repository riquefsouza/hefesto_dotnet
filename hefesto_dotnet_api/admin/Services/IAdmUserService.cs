using System.Collections.Generic;
using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.admin.VO;
using hefesto.base_hefesto.Services;

namespace hefesto.admin.Services
{
    public interface IAdmUserService : IBaseCrud<AdmUser, long>
    {
        void SetTransient(List<AdmUser> list);
        void SetTransient(AdmUser item);
        Task<AdmUser> Authenticate(string login, string password);
        bool VerifyPassword(string password, string hashPassword);

        List<UserVO> FindByLikeEmail(string email);
        Task<AdmUser> GetUser(string login, string name, string email, bool auditar);
    }
}