using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.admin.VO
{
    public class AuthenticatedUserVO : IEquatable<AuthenticatedUserVO>
	{
		public string UserName { get; set; }

		public string DisplayName { get; set; }

		public string Email { get; set; }

		public List<PermissionVO> ListPermission { get; set; }

		public List<MenuVO> ListMenus { get; set; }

		public List<MenuVO> ListAdminMenus { get; set; }

		public UserVO User { get; set; }

		public bool ModeTest { get; set; }

		public string ModeTestLogin { get; set; }

		public string ModeTestLoginVirtual { get; set; }

		public AuthenticatedUserVO()
		{
			this.ListPermission = new List<PermissionVO>();
			this.User = new UserVO();
			this.ListMenus = new List<MenuVO>();
			this.ListAdminMenus = new List<MenuVO>();

			Clean();

			this.ModeTest = false;
			this.ModeTestLogin = "";
			this.ModeTestLoginVirtual = "";
		}

		public void Clean()
		{
			this.UserName = "";
			this.DisplayName = "";
			this.Email = "";
			this.ListPermission.Clear();
			this.ListMenus.Clear();
			this.ListAdminMenus.Clear();
			this.User.Clean();
			this.ModeTestLogin = "";
			this.ModeTestLoginVirtual = "";
		}

		public AuthenticatedUserVO(string userName)
		{
			this.UserName = userName;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as AuthenticatedUserVO);
		}

		public bool Equals(AuthenticatedUserVO other)
		{
			if (other == null)
				return false;

			if (this.GetType() != other.GetType())
				return false;

			if (UserName == null)
			{
				if (other.UserName != null)
					return false;
			}
			else if (!UserName.Equals(other.UserName))
				return false;

			return true;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(UserName);
		}

		public ProfileVO GetProfile(long idProfile)
		{
			ProfileVO admProfile = null;
			foreach (PermissionVO permissaoVO in ListPermission)
			{
				if (permissaoVO.Profile.Id == idProfile)
				{
					admProfile = permissaoVO.Profile;
					break;
				}
			}
			return admProfile;
		}

		public bool IsProfile(long idProfile)
		{
			return (GetProfile(idProfile) != null);
		}

		public ProfileVO GetProfile(string profile)
		{
			ProfileVO admProfile = null;
			foreach (PermissionVO permissaoVO in ListPermission)
			{
				if (permissaoVO.Profile.Description.Equals(profile, StringComparison.OrdinalIgnoreCase))
				{
					admProfile = permissaoVO.Profile;
					break;
				}
			}
			return admProfile;
		}

		public bool IsProfile(string profile)
		{
			return (GetProfile(profile) != null);
		}

		public ProfileVO GetProfileGeneral()
		{
			ProfileVO admProfile = null;
			foreach (PermissionVO permissaoVO in ListPermission)
			{
				if (permissaoVO.Profile.General)
				{
					admProfile = permissaoVO.Profile;
					break;
				}
			}
			return admProfile;
		}

		public ProfileVO GetProfileAdministrator()
		{
			ProfileVO admProfile = null;
			foreach (PermissionVO permissaoVO in ListPermission)
			{
				if (permissaoVO.Profile.Administrator)
				{
					admProfile = permissaoVO.Profile;
					break;
				}
			}
			return admProfile;
		}

		public bool IsGeneral()
		{
			ProfileVO profile = this.GetProfileGeneral();
			if (profile != null)
			{
				return profile.General;
			}
			return false;
		}

		public bool IsAdministrator()
		{
			ProfileVO profile = this.GetProfileAdministrator();
			if (profile != null)
			{
				return profile.Administrator;
			}
			return false;
		}

		public PageVO GetPageByMenu(long idMenu)
		{
			PageVO admPage = null;

			if (ListMenus != null && ListMenus.Count > 0)
			{
				foreach (MenuVO admMenu in ListMenus)
				{
					admPage = admMenu.Page;
					break;
				}
			}

			if (ListAdminMenus != null && ListAdminMenus.Count > 0)
			{
				foreach (MenuVO admMenu in ListAdminMenus)
				{
					admPage = admMenu.Page;
					break;
				}
			}

			return admPage;
		}

		public bool HasPermission(string url, string tela)
		{

			if (url == null)
			{
				return false;
			}
			int pos = url.IndexOf("private");
			url = pos > -1 ? url.Substring(pos + 8, url.Length) : url;

			if (url.Equals(tela))
			{
				return true;
			}

			foreach (PermissionVO permissao in this.ListPermission)
			{
				foreach (PageVO admPage in permissao.Pages)
				{
					if (admPage.Url.Equals(url))
					{
						return true;
					}
				}
			}

			return false;
		}

		public MenuVO GetMenu(string sidMenu)
		{
			MenuVO menu = null;
			long idMenu = long.Parse(sidMenu);

			foreach (var item in ListMenus)
            {
				menu = item.SubMenus.Where(submenu => submenu.Id.Equals(idMenu)).First();
				if (menu != null)
                {
					break;
                }
			}

			if (menu == null)
			{
				foreach (var item in ListAdminMenus)
				{
					menu = item.SubMenus.Where(submenu => submenu.Id.Equals(idMenu)).First();
					if (menu != null)
					{
						break;
					}
				}
			}

			return menu;
		}

	}
}
