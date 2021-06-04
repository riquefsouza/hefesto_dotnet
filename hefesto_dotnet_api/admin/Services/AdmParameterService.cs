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

            return new BasePaged<AdmParameter>(pagedData,
                BasePaging.of(validFilter, totalRecords, _uriService, route));
        }
    }
}