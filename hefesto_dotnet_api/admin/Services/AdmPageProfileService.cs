using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hefesto.admin.Models;

namespace hefesto.admin.Services
{
    public class AdmPageProfileService : IAdmPageProfileService
    {
        private readonly IDbContextFactory<dbhefestoContext> _contextFactory;

        public AdmPageProfileService(IDbContextFactory<dbhefestoContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void SetTransient(List<AdmPageProfile> list)
        {
            foreach (var item in list)
            {
                SetTransient(item);
            }
        }

        public void SetTransient(AdmPageProfile item)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                item.AdmPage = _context.AdmPages.Find(item.IdPage);
                item.AdmProfile = _context.AdmProfiles.Find(item.IdProfile);
            }
        }

        public async Task<List<AdmPageProfile>> FindAll()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var listAdmPageProfile = await _context.AdmPageProfiles.ToListAsync();
                SetTransient(listAdmPageProfile);
                return listAdmPageProfile;
            }
        }

        public List<AdmProfile> GetProfilesByPage(long admPageId)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = _context.AdmPageProfiles.AsQueryable();
                query = _context.AdmPageProfiles.Where(adm => adm.IdPage == admPageId);
                var listAdmPageProfile = query.ToList();

                List<AdmProfile> lista = new List<AdmProfile>();

                foreach (var item in listAdmPageProfile)
                {
                    SetTransient(item);
                    lista.Add(item.AdmProfile);
                }

                return lista;
            }
        }

        public List<AdmPage> GetPagesByProfile(long admProfileId)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = _context.AdmPageProfiles.AsQueryable();
                query = _context.AdmPageProfiles.Where(adm => adm.IdProfile == admProfileId);
                var listAdmPageProfile = query.ToList();

                List<AdmPage> lista = new List<AdmPage>();

                foreach (var item in listAdmPageProfile)
                {
                    SetTransient(item);
                    lista.Add(item.AdmPage);
                }

                return lista;
            }
        }
    }
}