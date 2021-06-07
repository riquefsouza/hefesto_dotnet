using System;
using System.Linq;
using System.Collections.Generic;
using hefesto.admin.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hefesto.base_hefesto.Pagination;
using hefesto.base_hefesto.Services;
using hefesto.base_hefesto.Models;

namespace hefesto.admin.Services
{
    public class AdmProfileService : IAdmProfileService
    {
        private readonly dbhefestoContext _context;
        private readonly IAdmPageProfileService _servicePageProfile;
        private readonly IAdmUserProfileService _serviceUserProfile;
        private readonly IAdmMenuService _serviceMenu;
        private readonly IUriService _uriService;

        public AdmProfileService(dbhefestoContext context, IUriService uriService,
            IAdmPageProfileService servicePageProfile, 
            IAdmUserProfileService serviceUserProfile,
            IAdmMenuService serviceMenu)
        {
            _context = context;
            _uriService = uriService;
            _servicePageProfile = servicePageProfile;
            _serviceUserProfile = serviceUserProfile;
            _serviceMenu = serviceMenu;
        }

        public async Task<List<AdmProfile>> FindProfilesByPage(long pageId)
        {
            List<AdmProfile> listProfiles = _servicePageProfile.GetProfilesByPage(pageId);
            SetTransient(listProfiles);
            return await Task.FromResult(listProfiles);
        }

        public async Task<List<AdmProfile>> FindProfilesByUser(long userId)
        {
            List<AdmProfile> listProfiles = _serviceUserProfile.GetProfilesByUser(userId);
            SetTransient(listProfiles);
            return await Task.FromResult(listProfiles);
        }

        public void SetTransient(List<AdmProfile> list)
        {
            foreach (var item in list)
            {
                SetTransient(item);
            }
        }

        public void SetTransient(AdmProfile item)
        {
            List<AdmPage> listPages = _servicePageProfile.GetPagesByProfile(item.Id);
            listPages.ForEach(page => item.AdmPages.Add(page));

            List<string> listProfilePages = new List<string>();
            listPages.ForEach(page => listProfilePages.Add(page.Description));
            item.ProfilePages = String.Join(",", listProfilePages);

            List<AdmUser> listUsers = _serviceUserProfile.GetUsersByProfile(item.Id);
            listUsers.ForEach(user => item.AdmUsers.Add(user));

            List<string> listProfileUsers = new List<string>();
            listUsers.ForEach(user => listProfileUsers.Add(user.Name));
            item.ProfileUsers = String.Join(",", listProfileUsers);

        }

        public async Task<BasePaged<AdmProfile>> GetPage(string route, PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.pageNumber, filter.size, filter.sort,
                filter.columnOrder, filter.columnTitle);

            var pagedData = await _context.AdmProfiles
                .Skip((validFilter.pageNumber - 1) * validFilter.size)
                .Take(validFilter.size)
                .ToListAsync();
            var totalRecords = await _context.AdmProfiles.CountAsync();
            this.SetTransient(pagedData);

            return new BasePaged<AdmProfile>(pagedData,
                BasePaging.of(validFilter, totalRecords, _uriService, route));
        }

        public async Task<List<AdmProfile>> FindAll()
        {
            var listObj = await _context.AdmProfiles.ToListAsync();
            this.SetTransient(listObj);
            return listObj;
        }

        public async Task<AdmProfile> FindById(long? id)
        {
            var obj = await _context.AdmProfiles.FindAsync(id);

            if (obj != null)
            {
                this.SetTransient(obj);
            }

            return obj;
        }

        public async Task<bool> Update(long id, AdmProfile obj)
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

        public async Task<AdmProfile> Insert(AdmProfile obj)
        {
            obj.Id = this.GetNextSequenceValue();

            _context.AdmProfiles.Add(obj);
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
            var obj = await _context.AdmProfiles.FindAsync(id);
            if (obj == null)
            {
                return false;
            }

            _context.AdmProfiles.Remove(obj);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool Exists(long id)
        {
            return _context.AdmProfiles.Any(e => e.Id == id);
        }

        private long GetNextSequenceValue()
        {
            var rawQuery = _context.Set<SequenceValue>().FromSqlRaw("select nextval('public.adm_profile_seq') as Value;");
            var nextVal = rawQuery.AsEnumerable().First().Value;

            return nextVal;
        }

        public List<AdmMenu> findMenuByIdProfiles(List<long> listaIdProfile, AdmMenu admMenu){
            /*
            findMenuByIdProfiles:
                select distinct admmenus3_.*
                from adm_profile admprofile0_ 
                inner join adm_page_profile admpages1_ on admprofile0_.prf_seq=admpages1_.pgl_prf_seq 
                inner join adm_page admpage2_ on admpages1_.pgl_pag_seq=admpage2_.pag_seq 
                inner join adm_menu admmenus3_ on admpage2_.pag_seq=admmenus3_.mnu_pag_seq 
                where admprofile0_.prf_seq in (? , ?) and admmenus3_.mnu_seq > 9 and admmenus3_.mnu_parent_seq=? 
                order by admmenus3_.mnu_seq, admmenus3_.mnu_order
            */

            var findMenuByIdProfiles =
            from pf in _context.AdmProfiles
            join pp in _context.AdmPageProfiles on pf.Id equals pp.IdProfile
            join page in _context.AdmPages on pp.IdPage equals page.Id
            join mnu in _context.AdmMenus on page.Id equals mnu.IdPage
            where listaIdProfile.Contains(pf.Id) && mnu.Id > 9 && mnu.IdMenuParent == admMenu.Id
            orderby mnu.Id, mnu.Order
            select mnu;

            List<AdmMenu> lista = findMenuByIdProfiles.Distinct().ToList();

            return lista;
        }

        public List<AdmMenu> findAdminMenuByIdProfiles(List<long> listaIdProfile, AdmMenu admMenu){
            /*
            findAdminMenuByIdProfiles:
                select distinct admmenus3_.*
                from adm_profile admprofile0_ 
                inner join adm_page_profile admpages1_ on admprofile0_.prf_seq=admpages1_.pgl_prf_seq 
                inner join adm_page admpage2_ on admpages1_.pgl_pag_seq=admpage2_.pag_seq 
                inner join adm_menu admmenus3_ on admpage2_.pag_seq=admmenus3_.mnu_pag_seq 
                where admprofile0_.prf_seq in (? , ?) and admmenus3_.mnu_seq<=9 and admmenus3_.mnu_parent_seq=? 
                order by admmenus3_.mnu_seq, admmenus3_.mnu_order
            */

            var findAdminMenuByIdProfiles =
            from pf in _context.AdmProfiles
            join pp in _context.AdmPageProfiles on pf.Id equals pp.IdProfile
            join page in _context.AdmPages on pp.IdPage equals page.Id
            join mnu in _context.AdmMenus on page.Id equals mnu.IdPage
            where listaIdProfile.Contains(pf.Id) && mnu.Id <= 9 && mnu.IdMenuParent == admMenu.Id
            orderby mnu.Id, mnu.Order
            select mnu;

            List<AdmMenu> lista = findAdminMenuByIdProfiles.Distinct().ToList();

            return lista;
        }

        public List<AdmMenu> findMenuParentByIdProfiles(List<long> listaIdProfile){
            /*
            findMenuParentByIdProfiles:
                select distinct admmenu0_.* from adm_menu admmenu0_ 
                where admmenu0_.mnu_seq in (
                    select admmenus4_.mnu_parent_seq 
                    from adm_profile admprofile1_ 
                    inner join adm_page_profile admpages2_ on admprofile1_.prf_seq=admpages2_.pgl_prf_seq 
                    inner join adm_page admpage3_ on admpages2_.pgl_pag_seq=admpage3_.pag_seq 
                    inner join adm_menu admmenus4_ on admpage3_.pag_seq=admmenus4_.mnu_pag_seq 
                    where (admprofile1_.prf_seq in (? , ?)) and admmenus4_.mnu_seq > 9
                ) 
                order by admmenu0_.mnu_order, admmenu0_.mnu_seq
            */        

            var findMenuParentByIdProfiles_subquery =
            from pf in _context.AdmProfiles
            join pp in _context.AdmPageProfiles on pf.Id equals pp.IdProfile
            join page in _context.AdmPages on pp.IdPage equals page.Id
            join mnu in _context.AdmMenus on page.Id equals mnu.IdPage
            where listaIdProfile.Contains(pf.Id) && mnu.Id > 9
            select mnu.IdMenuParent;  

            List<long?> sublista = findMenuParentByIdProfiles_subquery.ToList();

            //var findMenuParentByIdProfiles =
            List<AdmMenu> lista = _context.AdmMenus
            .Where(u => sublista.Contains(u.Id))
            .OrderBy(u => u.Order).ThenBy(u => u.Id)
            .Distinct().ToList();

            foreach (AdmMenu admMenu in lista) {
                List<AdmMenu> plist = this.findMenuByIdProfiles(listaIdProfile, admMenu);
                _serviceMenu.SetTransientWithoutSubMenus(plist);
                _serviceMenu.SetTransientSubMenus(admMenu, plist);
            }
            return lista;
        }

        public List<AdmMenu> findAdminMenuParentByIdProfiles(List<long> listaIdProfile){
            /*
            findAdminMenuParentByIdProfiles: 
                select distinct admmenu0_.* from adm_menu admmenu0_ 
                where admmenu0_.mnu_seq in (
                    select admmenus4_.mnu_parent_seq 
                    from adm_profile admprofile1_ 
                    inner join adm_page_profile admpages2_ on admprofile1_.prf_seq=admpages2_.pgl_prf_seq 
                    inner join adm_page admpage3_ on admpages2_.pgl_pag_seq=admpage3_.pag_seq 
                    inner join adm_menu admmenus4_ on admpage3_.pag_seq=admmenus4_.mnu_pag_seq 
                    where admprofile1_.prf_seq in (? , ?) and admmenus4_.mnu_seq<=9
                ) 
                order by admmenu0_.mnu_seq, admmenu0_.mnu_order
            */
                
            var findAdminMenuParentByIdProfiles_subquery =
            from pf in _context.AdmProfiles
            join pp in _context.AdmPageProfiles on pf.Id equals pp.IdProfile
            join page in _context.AdmPages on pp.IdPage equals page.Id
            join mnu in _context.AdmMenus on page.Id equals mnu.IdPage
            where listaIdProfile.Contains(pf.Id) && mnu.Id <= 9
            select mnu.IdMenuParent;  
            
            List<long?> sublista = findAdminMenuParentByIdProfiles_subquery.ToList();

            //var findAdminMenuParentByIdProfiles =
            List<AdmMenu> lista = _context.AdmMenus
            .Where(u => sublista.Contains(u.Id))
            .OrderBy(u => u.Order).ThenBy(u => u.Id)
            .Distinct().ToList();

            foreach (AdmMenu admMenu in lista) {
                List<AdmMenu> plist = this.findAdminMenuByIdProfiles(listaIdProfile, admMenu);
                _serviceMenu.SetTransientWithoutSubMenus(plist);
                _serviceMenu.SetTransientSubMenus(admMenu, plist);
            }
            return lista;
        }

        public async Task<List<MenuItemDTO>> MountMenuItem(List<long> listaIdProfile)
        {
            List<MenuItemDTO> lista = new List<MenuItemDTO>();

			this.findMenuParentByIdProfiles(listaIdProfile).ForEach(menu => {			
                List<MenuItemDTO> item = new List<MenuItemDTO>();
                List<AdmMenu> admSubMenus = new List<AdmMenu>(menu.InverseAdmMenuParent);                

                admSubMenus.ForEach(submenu => {
                    MenuItemDTO submenuVO = new MenuItemDTO(submenu.Description, submenu.Url);
                    item.Add(submenuVO);
                });
                
                MenuItemDTO vo = new MenuItemDTO(menu.Description, menu.Url, item);
                lista.Add(vo);
            });
            
            this.findAdminMenuParentByIdProfiles(listaIdProfile).ForEach(menu => {			
                List<MenuItemDTO> item = new List<MenuItemDTO>();
                List<AdmMenu> admSubMenus = new List<AdmMenu>(menu.InverseAdmMenuParent);

                admSubMenus.ForEach(submenu => {
                    MenuItemDTO submenuVO = new MenuItemDTO(submenu.Description, submenu.Url);
                    item.Add(submenuVO);
                });
                
                MenuItemDTO vo = new MenuItemDTO(menu.Description, menu.Url, item);
                lista.Add(vo);
            });
		
    		return await Task.FromResult(lista);
        }

    }
}