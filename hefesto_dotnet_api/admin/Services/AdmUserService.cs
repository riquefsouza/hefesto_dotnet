using System;
using System.Linq;
using System.Collections.Generic;
using hefesto.admin.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hefesto.base_hefesto.Pagination;
using hefesto.base_hefesto.Services;
using BC = BCrypt.Net.BCrypt;

namespace hefesto.admin.Services
{
    public class AdmUserService : IAdmUserService
    {
        private readonly dbhefestoContext _context;

        private readonly IAdmUserProfileService _service;

        private readonly IUriService _uriService;

        public AdmUserService(dbhefestoContext context, IAdmUserProfileService service, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
            _service = service;
        }

        public void SetTransient(List<AdmUser> list)
        {
            foreach (var item in list)
            {
                SetTransient(item);
            }
        }

        public void SetTransient(AdmUser item)
        {
            var obj = _service.GetProfilesByUser(item.Id);                
            obj.ForEach(profile => item.AdmIdProfiles.Add(profile.Id));

            List<string> listUserProfiles = new List<string>();
            obj.ForEach(profile => listUserProfiles.Add(profile.Description));
            item.UserProfiles = String.Join(",", listUserProfiles);
        }

        public async Task<BasePaged<AdmUser>> GetPage(string route, PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.pageNumber, filter.size, filter.sort,
                filter.columnOrder, filter.columnTitle);

            var pagedData = await _context.AdmUsers
                .Skip((validFilter.pageNumber - 1) * validFilter.size)
                .Take(validFilter.size)
                .ToListAsync();
            var totalRecords = await _context.AdmUsers.CountAsync();

            return new BasePaged<AdmUser>(pagedData,
                BasePaging.of(validFilter, totalRecords, _uriService, route));
        }

        public async Task<AdmUser> Authenticate(string login, string password)
        {
            var admUser = await _context.AdmUsers.FirstOrDefaultAsync(u => u.Login == login);
            
            if (admUser != null){
                if (VerifyPassword(password, admUser.Password)){
                    return admUser;
                }
            }
            return null;
        }

        public bool VerifyPassword(string password, string hashPassword)
        {
            return BC.Verify(password, hashPassword);
        }

        public void Register(AdmUser model)
        {
            model.Password = BC.HashPassword(model.Password);
        }

    }
}