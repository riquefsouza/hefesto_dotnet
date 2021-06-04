using System.Collections.Generic;
using System.Threading.Tasks;
using hefesto.admin.Models;

namespace hefesto.admin.Services
{
    public interface IAdmUserProfileService
    {
        void SetTransient(List<AdmUserProfile> list);
        void SetTransient(AdmUserProfile item);
        Task<List<AdmUserProfile>> findAll();
        List<AdmProfile> GetProfilesByUser(long admUserId);
        List<AdmUser> GetUsersByProfile(long admProfileId);
    }
}