using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.admin.VO
{
    public class MenuVO : IEquatable<MenuVO>
    {
		public long Id { get; set; }

		public string Description { get; set; }

		public int? Order { get; set; }

		public long? IdPage { get; set; }

		public PageVO Page { get; set; }

		public MenuVO MenuParent { get; set; }

		private List<MenuVO> _subMenus;

		public List<MenuVO> SubMenus { 
			get {
				if (this._subMenus != null && this._subMenus.Count > 0)
				{
					this._subMenus.Sort(delegate (MenuVO o1, MenuVO o2)
					{
						if (o1 == null || o1.Order == -1 || o2 == null || o2.Order == -1)
						{
							return 0;
						}
						return IntegerCompareTo(o1.Order, o2.Order);
					});
				}
				return this._subMenus;
			}

			set
            {
				this._subMenus = value;
			}
		
		}

		public MenuVO()
		{
			this._subMenus = new List<MenuVO>();
			Clean();
		}

		public void Clean()
		{
			this.Id = -1;
			this.Description = null;
			this.Order = -1;
			this.IdPage = -1;
			this.Page = new PageVO();
			this.MenuParent = null;
			this._subMenus.Clear();

		}

		public MenuVO AddSubMenus(MenuVO subMenus)
		{
			_subMenus.Add(subMenus);
			subMenus.MenuParent = this;

			return subMenus;
		}

		public MenuVO RemoveSubMenus(MenuVO subMenus)
		{
			_subMenus.Remove(subMenus);
			subMenus.MenuParent = null;

			return subMenus;
		}

		public bool IsSubMenu()
		{
			return Page == null;
		}

		public string Url()
		{
			return this.Page != null ? this.Page.Url : null;
		}

		override public string ToString()
		{
			return this.Description;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as MenuVO);
		}

		public bool Equals(MenuVO other)
		{
			if (other == null)
				return false;

			if (this.GetType() != other.GetType())
				return false;

			if (MenuParent == null)
			{
				if (other.MenuParent != null)
					return false;
			}
			else if (!MenuParent.Equals(other.MenuParent))
				return false;
			if (Description == null)
			{
				if (other.Description != null)
					return false;
			}
			else if (!Description.Equals(other.Description))
				return false;
			if (Id == -1)
			{
				if (other.Id != -1)
					return false;
			}
			else if (!Id.Equals(other.Id))
				return false;

			return true;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Id, Description, MenuParent);
		}

		private int IntegerCompareTo(int? value, int? another)
        {
			//if (another == null) return 1;
			return (value < another ? -1 : (value == another ? 0 : 1));
		}
	}
}
