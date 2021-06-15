using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hefesto.admin.Models;
using hefesto.base_hefesto.Pagination;
using hefesto.base_hefesto.Services;

namespace hefesto.admin.Services
{
    public class AdmParameterCategoryService : IAdmParameterCategoryService
    {
        //private readonly dbhefestoContext _context;
        private readonly IDbContextFactory<dbhefestoContext> _contextFactory;

        private readonly IUriService _uriService;

        public AdmParameterCategoryService(IDbContextFactory<dbhefestoContext> contextFactory, IUriService uriService)
        {
            _contextFactory = contextFactory;
            _uriService = uriService;
        }

        public async Task<BasePaged<AdmParameterCategory>> GetPage(string route, PaginationFilter filter)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                //var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.pageNumber, filter.size, filter.sort,
                filter.columnOrder, filter.columnTitle);

                var pagedData = await _context.AdmParameterCategories
                    .Skip((validFilter.pageNumber - 1) * validFilter.size)
                    .Take(validFilter.size)
                    .ToListAsync();
                var totalRecords = await _context.AdmParameterCategories.CountAsync();

                return new BasePaged<AdmParameterCategory>(pagedData,
                    BasePaging.of(validFilter, totalRecords, _uriService, route));
            }
        }

        public async Task<List<AdmParameterCategory>> FindAll()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var listObj = await _context.AdmParameterCategories.ToListAsync();
                return listObj;
            }
        }

        public async Task<AdmParameterCategory> FindById(long? id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var obj = await _context.AdmParameterCategories.FindAsync(id);
                return obj;
            }
        }

        public async Task<bool> Update(long id, AdmParameterCategory obj)
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

        public async Task<AdmParameterCategory> Insert(AdmParameterCategory obj)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                obj.Id = this.GetNextSequenceValue();

                _context.AdmParameterCategories.Add(obj);
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
                var obj = await _context.AdmParameterCategories.FindAsync(id);
                if (obj == null)
                {
                    return false;
                }

                _context.AdmParameterCategories.Remove(obj);
                await _context.SaveChangesAsync();

                return true;
            }
        }

        public bool Exists(long id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return _context.AdmParameterCategories.Any(e => e.Id == id);
            }
        }

        public long GetNextSequenceValue()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var rawQuery = _context.Set<SequenceValue>()
                    .FromSqlRaw("select nextval('public.adm_parameter_category_seq') as Value;");
                var nextVal = rawQuery.AsEnumerable().First().Value;

                return nextVal;
            }
        }

    }
}