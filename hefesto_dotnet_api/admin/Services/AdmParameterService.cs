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
    public class AdmParameterService : IAdmParameterService
    {
        private readonly IDbContextFactory<dbhefestoContext> _contextFactory;

        private readonly IUriService _uriService;

        public AdmParameterService(IDbContextFactory<dbhefestoContext> contextFactory, IUriService uriService)
        {
            _contextFactory = contextFactory;
            _uriService = uriService;
        }

        public void SetTransient(List<AdmParameter> list)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                foreach (var item in list)
                {
                    item.AdmParameterCategory = _context.AdmParameterCategories.Find(item.IdParameterCategory);
                }
            }
        }

        public void SetTransient(AdmParameter item)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                item.AdmParameterCategory = _context.AdmParameterCategories.Find(item.IdParameterCategory);
            }
        }

        public async Task<BasePaged<AdmParameter>> GetPage(string route, PaginationFilter filter)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var validFilter = new PaginationFilter(filter.pageNumber, filter.size, filter.sort,
                filter.columnOrder, filter.columnTitle);

                var pagedData = await _context.AdmParameters
                    .Skip((validFilter.pageNumber - 1) * validFilter.size)
                    .Take(validFilter.size)
                    .ToListAsync();
                var totalRecords = await _context.AdmParameters.CountAsync();

                this.SetTransient(pagedData);

                return new BasePaged<AdmParameter>(pagedData,
                    BasePaging.of(validFilter, totalRecords, _uriService, route));
            }
        }

        public async Task<List<AdmParameter>> FindAll()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var listObj = await _context.AdmParameters.ToListAsync();
                this.SetTransient(listObj);
                return listObj;
            }
        }

        public async Task<AdmParameter> FindById(long? id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var obj = await _context.AdmParameters.FindAsync(id);

                if (obj != null)
                {
                    this.SetTransient(obj);
                }

                return obj;
            }
        }

        public async Task<bool> Update(long id, AdmParameter obj)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                if (obj.AdmParameterCategory != null)
                {
                    obj.IdParameterCategory = obj.AdmParameterCategory.Id;
                    obj.AdmParameterCategory = null;
                }

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

        public async Task<AdmParameter> Insert(AdmParameter obj)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                if (obj.AdmParameterCategory != null)
                {
                    obj.IdParameterCategory = obj.AdmParameterCategory.Id;
                    obj.AdmParameterCategory = null;
                }

                obj.Id = this.GetNextSequenceValue();

                _context.AdmParameters.Add(obj);
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
                var obj = await _context.AdmParameters.FindAsync(id);
                if (obj == null)
                {
                    return false;
                }

                _context.AdmParameters.Remove(obj);
                await _context.SaveChangesAsync();

                return true;
            }
        }

        public bool Exists(long id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return _context.AdmParameters.Any(e => e.Id == id);
            }
        }

        private long GetNextSequenceValue()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var rawQuery = _context.Set<SequenceValue>().FromSqlRaw("select nextval('public.adm_parameter_seq') as Value;");
                var nextVal = rawQuery.AsEnumerable().First().Value;

                return nextVal;
            }
        }

        public string GetValueByCode(string scode)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var svalue =
                    from p in _context.AdmParameters
                    where p.Code == scode
                    select p.Value;

                    return svalue.Distinct().First();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

    }
}