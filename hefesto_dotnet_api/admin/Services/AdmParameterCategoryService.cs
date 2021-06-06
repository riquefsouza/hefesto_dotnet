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
        private readonly dbhefestoContext _context;

        private readonly IUriService _uriService;

        public AdmParameterCategoryService(dbhefestoContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        public async Task<BasePaged<AdmParameterCategory>> GetPage(string route, PaginationFilter filter)
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

        public async Task<List<AdmParameterCategory>> FindAll()
        {
            var listObj = await _context.AdmParameterCategories.ToListAsync();
            return listObj;
        }

        public async Task<AdmParameterCategory> FindById(long? id)
        {
            var obj = await _context.AdmParameterCategories.FindAsync(id);
            return obj;
        }

        public async Task<bool> Update(long id, AdmParameterCategory obj)
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

        public async Task<AdmParameterCategory> Insert(AdmParameterCategory obj)
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

        public async Task<bool> Delete(long id)
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

        public bool Exists(long id)
        {
            return _context.AdmParameterCategories.Any(e => e.Id == id);
        }

        private long GetNextSequenceValue()
        {
            var rawQuery = _context.Set<SequenceValue>().FromSqlRaw("select nextval('public.adm_parameter_category_seq') as Value;");
            var nextVal = rawQuery.AsEnumerable().First().Value;

            return nextVal;
        }

    }
}