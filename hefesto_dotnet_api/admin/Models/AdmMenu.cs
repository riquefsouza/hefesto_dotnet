using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using hefesto.admin.VO;

#nullable disable

namespace hefesto.admin.Models
{
    public partial class AdmMenu
    {
        public AdmMenu()
        {
            AdmSubMenus = new HashSet<AdmMenu>();
        }

        public long Id { get; set; }

        [Required(ErrorMessage = "The field Description is required")]
        public string Description { get; set; }
        public long? IdMenuParent { get; set; }
        public long? IdPage { get; set; }
        public int? Order { get; set; }

        public virtual AdmPage AdmPage { get; set; }
        public virtual AdmMenu AdmMenuParent { get; set; }

        [JsonIgnore]
        public virtual ICollection<AdmMenu> AdmSubMenus { get; set; }
        
        [JsonIgnore]
        public virtual string Url { get; set; }

		private string NomeRecursivo(AdmMenu m)
		{
			return m.AdmPage == null ? m.Description
					: m.AdmMenuParent != null ? NomeRecursivo(m.AdmMenuParent) + " -> " + m.Description : "";
		}

		public string NomeRecursivo()
		{
			return this.NomeRecursivo(this);
		}

		public MenuVO ToMenuVO()
		{
			MenuVO m = new MenuVO();

			m.Id = this.Id;
			m.Description = this.Description;
			m.Order = this.Order;
			m.IdPage = this.IdPage;
			if (AdmPage != null)
			{
				m.Page = AdmPage.ToPageVO();
			}
			foreach (AdmMenu admSubMenu in AdmSubMenus)
			{
				m.SubMenus.Add(admSubMenu.ToMenuVO());
			}

			return m;
		}
	}
}
