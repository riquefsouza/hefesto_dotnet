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
            this.SetTransient(pagedData);

            return new BasePaged<AdmUser>(pagedData,
                BasePaging.of(validFilter, totalRecords, _uriService, route));
        }

        public async Task<List<AdmUser>> FindAll()
        {
            var listObj = await _context.AdmUsers.ToListAsync();
            this.SetTransient(listObj);
            return listObj;
        }

        public async Task<AdmUser> FindById(long? id)
        {
            var obj = await _context.AdmUsers.FindAsync(id);

            if (obj != null)
            {
                this.SetTransient(obj);
            }

            return obj;
        }

        public async Task<bool> Update(long id, AdmUser obj)
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

        public async Task<AdmUser> Insert(AdmUser obj)
        {
            obj.Id = this.GetNextSequenceValue();

            _context.AdmUsers.Add(obj);
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
            var obj = await _context.AdmUsers.FindAsync(id);
            if (obj == null)
            {
                return false;
            }

            _context.AdmUsers.Remove(obj);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool Exists(long id)
        {
            return _context.AdmUsers.Any(e => e.Id == id);
        }

        private long GetNextSequenceValue()
        {
            var rawQuery = _context.Set<SequenceValue>().FromSqlRaw("select nextval('public.adm_user_seq') as Value;");
            var nextVal = rawQuery.AsEnumerable().First().Value;

            return nextVal;
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