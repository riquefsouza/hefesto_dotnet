using System;
using System.Linq;
using System.Collections.Generic;
using hefesto.admin.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hefesto.base_hefesto.Models;
using hefesto.admin.Services;

namespace hefesto.base_hefesto.Services
{
    public class SystemService : ISystemService
    {
        private readonly dbhefestoContext _context;
        private readonly IAdmMenuService _serviceMenu;

        public SystemService(dbhefestoContext context, IAdmMenuService serviceMenu)
        {
            _context = context;
            _serviceMenu = serviceMenu;
        }

        public List<AdmMenu> findMenuByIdProfiles(List<long> listaIdProfile, AdmMenu admMenu)
        {
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

        public List<AdmMenu> findAdminMenuByIdProfiles(List<long> listaIdProfile, AdmMenu admMenu)
        {
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

        public List<AdmMenu> findMenuParentByIdProfiles(List<long> listaIdProfile)
        {
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

            foreach (AdmMenu admMenu in lista)
            {
                List<AdmMenu> plist = this.findMenuByIdProfiles(listaIdProfile, admMenu);
                _serviceMenu.SetTransientWithoutSubMenus(plist);
                _serviceMenu.SetTransientSubMenus(admMenu, plist);
            }
            return lista;
        }

        public List<AdmMenu> findAdminMenuParentByIdProfiles(List<long> listaIdProfile)
        {
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

            foreach (AdmMenu admMenu in lista)
            {
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
