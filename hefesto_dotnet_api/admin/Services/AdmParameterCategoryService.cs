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
    }
}