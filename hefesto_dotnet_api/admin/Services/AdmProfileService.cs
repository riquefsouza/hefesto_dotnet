using System;
using System.Linq;
using System.Collections.Generic;
using hefesto.admin.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hefesto.base_hefesto.Pagination;
using hefesto.base_hefesto.Services;
using hefesto.base_hefesto.Models;

namespace hefesto.admin.Services
{
    public class AdmProfileService : IAdmProfileService
    {
        private readonly dbhefestoContext _context;
        private readonly IAdmPageProfileService _servicePageProfile;
        private readonly IAdmUserProfileService _serviceUserProfile;
        private readonly IAdmMenuService _serviceMenu;
        private readonly IUriService _uriService;

        public AdmProfileService(dbhefestoContext context, IUriService uriService,
            IAdmPageProfileService servicePageProfile, 
            IAdmUserProfileService serviceUserProfile,
            IAdmMenuService serviceMenu)
        {
            _context = context;
            _uriService = uriService;
            _servicePageProfile = servicePageProfile;
            _serviceUserProfile = serviceUserProfile;
            _serviceMenu = serviceMenu;
        }

        public async Task<List<AdmProfile>> FindProfilesByPage(long pageId)
        {
            List<AdmProfile> listProfiles = _servicePageProfile.GetProfilesByPage(pageId);
            SetTransient(listProfiles);
            return await Task.FromResult(listProfiles);
        }

        public async Task<List<AdmProfile>> FindProfilesByUser(long userId)
        {
            List<AdmProfile> listProfiles = _serviceUserProfile.GetProfilesByUser(userId);
            SetTransient(listProfiles);
            return await Task.FromResult(listProfiles);
        }

        public void SetTransient(List<AdmProfile> list)
        {
            foreach (var item in list)
            {
                SetTransient(item);
            }
        }

        public void SetTransient(AdmProfile item)
        {
            List<AdmPage> listPages = _servicePageProfile.GetPagesByProfile(item.Id);
            listPages.ForEach(page => item.AdmPages.Add(page));

            List<string> listProfilePages = new List<string>();
            listPages.ForEach(page => listProfilePages.Add(page.Description));
            item.ProfilePages = String.Join(",", listProfilePages);

            List<AdmUser> listUsers = _serviceUserProfile.GetUsersByProfile(item.Id);
            listUsers.ForEach(user => item.AdmUsers.Add(user));

            List<string> listProfileUsers = new List<string>();
            listUsers.ForEach(user => listProfileUsers.Add(user.Name));
            item.ProfileUsers = String.Join(",", listProfileUsers);

        }

        public async Task<BasePaged<AdmProfile>> GetPage(string route, PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.pageNumber, filter.size, filter.sort,
                filter.columnOrder, filter.columnTitle);

            var pagedData = await _context.AdmProfiles
                .Skip((validFilter.pageNumber - 1) * validFilter.size)
                .Take(validFilter.size)
                .ToListAsync();
            var totalRecords = await _context.AdmProfiles.CountAsync();
            this.SetTransient(pagedData);

            return new BasePaged<AdmProfile>(pagedData,
                BasePaging.of(validFilter, totalRecords, _uriService, route));
        }

        public async Task<List<AdmProfile>> FindAll()
        {
            var listObj = await _context.AdmProfiles.ToListAsync();
            this.SetTransient(listObj);
            return listObj;
        }

        public async Task<AdmProfile> FindById(long? id)
        {
            var obj = await _context.AdmProfiles.FindAsync(id);

            if (obj != null)
            {
                this.SetTransient(obj);
            }

            return obj;
        }

        public async Task<bool> Update(long id, AdmProfile obj)
        {
            _context.Entry(obj).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.Exists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<AdmProfile> Insert(AdmProfile obj)
        {
            obj.Id = this.GetNextSequenceValue();

            _context.AdmProfiles.Add(obj);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (this.Exists(obj.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return obj;
        }

        public async Task<bool> Delete(long id)
        {
            var obj = await _context.AdmProfiles.FindAsync(id);
            if (obj == null)
            {
                return false;
            }

            _context.AdmProfiles.Remove(obj);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool Exists(long id)
        {
            return _context.AdmProfiles.Any(e => e.Id == id);
        }

        private long GetNextSequenceValue()
        {
            var rawQuery = _context.Set<SequenceValue>().FromSqlRaw("select nextval('public.adm_profile_seq') as Value;");
            var nextVal = rawQuery.AsEnumerable().First().Value;

            return nextVal;
        }

    }
}