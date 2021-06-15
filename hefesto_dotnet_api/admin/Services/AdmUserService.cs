using System;
using System.Linq;
using System.Collections.Generic;
using hefesto.admin.Models;
using hefesto.admin.VO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hefesto.base_hefesto.Pagination;
using hefesto.base_hefesto.Services;
using BC = BCrypt.Net.BCrypt;

namespace hefesto.admin.Services
{
    public class AdmUserService : IAdmUserService
    {
        private readonly IDbContextFactory<dbhefestoContext> _contextFactory;

        private readonly IAdmUserProfileService _service;

        private readonly IUriService _uriService;

        public AdmUserService(IDbContextFactory<dbhefestoContext> contextFactory, 
            IAdmUserProfileService service, IUriService uriService)
        {
            _contextFactory = contextFactory;
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
            if (item != null)
            {
                var obj = _service.GetProfilesByUser(item.Id);
                obj.ForEach(profile => item.AdmIdProfiles.Add(profile.Id));

                List<string> listUserProfiles = new List<string>();
                obj.ForEach(profile => listUserProfiles.Add(profile.Description));
                item.UserProfiles = String.Join(",", listUserProfiles);
            }
        }

        public async Task<BasePaged<AdmUser>> GetPage(string route, PaginationFilter filter)
        {
            using (var _context = _contextFactory.CreateDbContext())
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
        }

        public async Task<List<AdmUser>> FindAll()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var listObj = await _context.AdmUsers.ToListAsync();
                this.SetTransient(listObj);
                return listObj;
            }
        }

        public async Task<AdmUser> FindById(long? id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var obj = await _context.AdmUsers.FindAsync(id);

                if (obj != null)
                {
                    this.SetTransient(obj);
                }

                return obj;
            }
        }

        public async Task<bool> Update(long id, AdmUser obj)
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

        public async Task<AdmUser> Insert(AdmUser obj)
        {
            using (var _context = _contextFactory.CreateDbContext())
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
        }

        public async Task<bool> Delete(long id)
        {
            using (var _context = _contextFactory.CreateDbContext())
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
        }

        public bool Exists(long id)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return _context.AdmUsers.Any(e => e.Id == id);
            }
        }

        private long GetNextSequenceValue()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var rawQuery = _context.Set<SequenceValue>().FromSqlRaw("select nextval('public.adm_user_seq') as Value;");
                var nextVal = rawQuery.AsEnumerable().First().Value;

                return nextVal;
            }
        }

        public async Task<AdmUser> Authenticate(string login, string password)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var admUser = await _context.AdmUsers.FirstOrDefaultAsync(u => u.Login == login);

                if (admUser != null)
                {
                    if (VerifyPassword(password, admUser.Password))
                    {
                        return admUser;
                    }
                }
                return null;
            }
        }

        public bool VerifyPassword(string password, string hashPassword)
        {
            return BC.Verify(password, hashPassword);
        }

        public void Register(AdmUser model)
        {
            model.Password = BC.HashPassword(model.Password);
        }

        private List<UserVO> ToVO(List<AdmUser> listaOrigem)
        {
            List<UserVO> lista = new List<UserVO>();
            foreach (AdmUser item in listaOrigem)
            {
                lista.Add(item.ToUserVO());
            }
            return lista;
        }

        public AdmUser FindByLogin(string login)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = _context.AdmUsers.AsQueryable();
                query = _context.AdmUsers.Where(adm => adm.Login.Equals(login)).Distinct();

                var obj = query.FirstOrDefault();
                this.SetTransient(obj);

                return obj;
            }
        }

        public List<AdmUser> FindAdmUserByLikeEmail(string email)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = _context.AdmUsers.AsQueryable();
                query = _context.AdmUsers.Where(adm => adm.Email.Contains(email)).Distinct();

                var listObj = query.ToList();
                this.SetTransient(listObj);

                return listObj;
            }
        }

        public List<UserVO> FindByLikeEmail(string email)
        {
            List<AdmUser> listaOrigem = this.FindAdmUserByLikeEmail(email);
            List<UserVO> lista = ToVO(listaOrigem);

            return lista;
        }

        public async Task<AdmUser> GetUser(string login, string name, string email, bool auditar)
        {
            AdmUser user;
            user = new AdmUser();
            user.Id = -1;
            user.Login = login;
            user.Name = name;
            user.Email = email;
            //user.Ip = ;

            if (auditar)
            {
                await this.Insert(user);
            }

            return user;
        }
    }
}