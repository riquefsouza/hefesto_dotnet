using System.Collections.Generic;
using System.Threading.Tasks;
using hefesto.admin.Models;

namespace hefesto.admin.Services
{
    public interface IAdmPageProfileService
    {
        void SetTransient(List<AdmPageProfile> list);
        void SetTransient(AdmPageProfile item);
        Task<List<AdmPageProfile>> findAll();
        List<AdmProfile> GetProfilesByPage(long admPageId);
        List<AdmPage> GetPagesByProfile(long admProfileId);
    }
}