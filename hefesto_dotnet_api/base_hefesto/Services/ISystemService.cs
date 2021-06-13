using System;
using System.Collections.Generic;
using System.Linq;
using hefesto.base_hefesto.Models;
using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.admin.VO;

namespace hefesto.base_hefesto.Services
{
    public interface ISystemService
    {
        List<AdmMenu> FindMenuByIdProfiles(List<long> listaIdProfile, AdmMenu admMenu);
        List<AdmMenu> FindAdminMenuByIdProfiles(List<long> listaIdProfile, AdmMenu admMenu);
        List<AdmMenu> FindMenuParentByIdProfiles(List<long> listaIdProfile);
        List<AdmMenu> FindAdminMenuParentByIdProfiles(List<long> listaIdProfile);
        Task<List<MenuItemDTO>> MountMenuItem(List<long> listaIdProfile);

        List<MenuVO> FindMenuParentByProfile(List<long> listaIdProfile);
        List<MenuVO> FindAdminMenuParentByProfile(List<long> listaIdProfile);

        bool Authenticate(UserVO admUser);
        List<MenuVO> GetListaMenus();
        List<MenuVO> GetListaAdminMenus();
        PageVO GetPagina(long idMenu);
        AuthenticatedUserVO GetAuthenticatedUser();
    }
}
