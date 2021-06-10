using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.admin.VO
{
    public class PermissionVO
    {
		public ProfileVO Profile { get; set; }

		public List<PageVO> Pages { get; set; }

		public PermissionVO()
		{
			this.Pages = new List<PageVO>();
			Clean();
		}

		public void Clean()
		{
			this.Profile = new ProfileVO();
			this.Pages.Clear();
		}

		override public string ToString()
		{
			return "PermissionVO [profile=" + Profile + ", pages=" + Pages + "]";
		}
	}
}
