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
        private readonly dbhefestoContext _context;

        private readonly IUriService _uriService;

        public AdmParameterService(dbhefestoContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        public void SetTransient(List<AdmParameter> list)
        {
            foreach (var item in list)
            {
                item.AdmParameterCategory = _context.AdmParameterCategories.Find(item.IdParameterCategory); 
            }
        }

        public void SetTransient(AdmParameter item)
        {
            item.AdmParameterCategory = _context.AdmParameterCategories.Find(item.IdParameterCategory); 
        }

        public async Task<BasePaged<AdmParameter>> GetPage(string route, PaginationFilter filter)
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

        public async Task<List<AdmParameter>> FindAll()
        {
            var listObj = await _context.AdmParameters.ToListAsync();
            this.SetTransient(listObj);
            return listObj;
        }

        public async Task<AdmParameter> FindById(long? id)
        {
            var obj = await _context.AdmParameters.FindAsync(id);

            if (obj != null)
            {
                this.SetTransient(obj);
            }

            return obj;
        }

        public async Task<bool> Update(long id, AdmParameter obj)
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

        public async Task<AdmParameter> Insert(AdmParameter obj)
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

        public async Task<bool> Delete(long id)
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

        public bool Exists(long id)
        {
            return _context.AdmParameters.Any(e => e.Id == id);
        }

        private long GetNextSequenceValue()
        {
            var rawQuery = _context.Set<SequenceValue>().FromSqlRaw("select nextval('public.adm_parameter_seq') as Value;");
            var nextVal = rawQuery.AsEnumerable().First().Value;

            return nextVal;
        }

    }
}