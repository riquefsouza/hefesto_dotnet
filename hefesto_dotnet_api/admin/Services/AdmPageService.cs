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
        private readonly dbhefestoContext _context;

        private readonly IAdmPageProfileService _service;

        private readonly IUriService _uriService;

        public AdmPageService(dbhefestoContext context, IAdmPageProfileService service, IUriService uriService)
        {
            _context = context;
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
            var validFilter = new PaginationFilter(filter.pageNumber, filter.size, filter.sort,
                filter.columnOrder, filter.columnTitle);

            var pagedData = await _context.AdmPages
                .Skip((validFilter.pageNumber - 1) * validFilter.size)
                .Take(validFilter.size)
                .ToListAsync();
            var totalRecords = await _context.AdmPages.CountAsync();

            return new BasePaged<AdmPage>(pagedData,
                BasePaging.of(validFilter, totalRecords, _uriService, route));
        }

    }
}