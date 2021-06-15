using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hefesto.admin.Models;

namespace hefesto.admin.Services
{
    public class AdmUserProfileService : IAdmUserProfileService
    {
        private readonly IDbContextFactory<dbhefestoContext> _contextFactory;

        public AdmUserProfileService(IDbContextFactory<dbhefestoContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void SetTransient(List<AdmUserProfile> list)
        {
            foreach (var item in list)
            {
                SetTransient(item);
            }
        }

        public void SetTransient(AdmUserProfile item)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                item.AdmUser = _context.AdmUsers.Find(item.IdUser);
                item.AdmProfile = _context.AdmProfiles.Find(item.IdProfile);
            }
        }

        public async Task<List<AdmUserProfile>> FindAll()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var listAdmUserProfile = await _context.AdmUserProfiles.ToListAsync();
                SetTransient(listAdmUserProfile);
                return listAdmUserProfile;
            }
        }

        public List<AdmProfile> GetProfilesByUser(long admUserId)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = _context.AdmUserProfiles.AsQueryable();
                query = _context.AdmUserProfiles.Where(adm => adm.IdUser == admUserId);
                var listAdmUserProfile = query.ToList();

                List<AdmProfile> lista = new List<AdmProfile>();

                foreach (var item in listAdmUserProfile)
                {
                    SetTransient(item);
                    lista.Add(item.AdmProfile);
                }

                return lista;
            }
        }

        public List<AdmUser> GetUsersByProfile(long admProfileId)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = _context.AdmUserProfiles.AsQueryable();
                query = _context.AdmUserProfiles.Where(adm => adm.IdProfile == admProfileId);
                var listAdmUserProfile = query.ToList();

                List<AdmUser> lista = new List<AdmUser>();

                foreach (var item in listAdmUserProfile)
                {
                    SetTransient(item);
                    lista.Add(item.AdmUser);
                }

                return lista;
            }
        }

    }
}