using System;
using System.Linq;
using System.Collections.Generic;
using hefesto.admin.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hefesto.base_hefesto.Pagination;
using hefesto.base_hefesto.Services;

namespace hefesto.admin.Services
{
    public class AdmPageService : IAdmPageService
    {
        private readonly IDbContextFactory<dbhefestoContext> _contextFactory;

        private readonly IAdmPageProfileService _service;

        private readonly IUriService _uriService;

        public AdmPageService(IDbContextFactory<dbhefestoContext> contextFactory, IAdmPageProfileService service, IUriService uriService)
        {
            _contextFactory = contextFactory;
            _uriService = uriService;
            _service = service;
        }

        public void SetTransient(List<AdmPage> list)
        {
            foreach (var item in list)
            {
                SetTransient(item);
            }
        }

        public void SetTransient(AdmPage item)
        {
            var obj = _service.GetProfilesByPage(item.Id);
            obj.ForEach(profile => item.AdmIdProfiles.Add(profile.Id));

            List<string> listPageProfiles = new List<string>();
            obj.ForEach(profile => listPageProfiles.Add(profile.Description));
            item.PageProfiles = String.Join(",", listPageProfiles);
        }

        public async Task<BasePaged<AdmPage>> GetPage(string route, PaginationFilter filter)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var validFilter = new PaginationFilter(filter.pageNumber, filter.size, filter.sort,
                filter.columnOrder, filter.columnTitle);

                var pagedData = await _context.AdmPages
                    .Skip((validFilter.pageNumber - 1) * validFilter.size)
                    .Take(validFilter.size)
                    .ToListAsync();
                var totalRecords = await _context.AdmPages.CountAsync();
                this.SetTransient(pagedData);

                return new BasePaged<AdmPage>(pagedData,
                    BasePaging.of(validFilter, totalRecords, _uriService, route));
            }
        }

        public async Task<List<AdmPage>> FindAll()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var listObj = await _context.AdmPages.ToListAsync();
                this.SetTransient(listObj);
                return listObj;
            }
        }

        public async Task<AdmPage> FindById(long? id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var obj = await _context.AdmPages.FindAsync(id);

                if (obj != null)
                {
                    this.SetTransient(obj);
                }

                return obj;
            }
        }

        public async Task<bool> Update(long id, AdmPage obj)
        {
            using (var _context = _contextFactory.CreateDbContext())
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
        }

        public async Task<AdmPage> Insert(AdmPage obj)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                obj.Id = this.GetNextSequenceValue();

                _context.AdmPages.Add(obj);
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
        }

        public async Task<bool> Delete(long id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var obj = await _context.AdmPages.FindAsync(id);
                if (obj == null)
                {
                    return false;
                }

                _context.AdmPages.Remove(obj);
                await _context.SaveChangesAsync();

                return true;
            }
        }

        public bool Exists(long id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return _context.AdmPages.Any(e => e.Id == id);
            }
        }

        private long GetNextSequenceValue()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var rawQuery = _context.Set<SequenceValue>().FromSqlRaw("select nextval('public.adm_page_seq') as Value;");
                var nextVal = rawQuery.AsEnumerable().First().Value;

                return nextVal;
            }
        }
    }
}