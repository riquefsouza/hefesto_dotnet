using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.admin.VO
{
    public class PageVO
    {
		public long Id { get; set; }

		public string Description { get; set; }

		public string Url { get; set; }

		public List<ProfileVO> Profiles { get; set; }

		public PageVO()
		{
			this.Profiles = new List<ProfileVO>();
			Clean();
		}

		public void Clean()
		{
			this.Id = -1;
			this.Description = null;
			this.Url = null;
			this.Profiles.Clear();
		}

		override public string ToString()
		{
			return this.Url;
		}

		public string ProfilesPage()
		{
			string ret = "";
			foreach (ProfileVO item in Profiles)
			{
				ret = ret + item.Description + ", ";
			}
			if (ret != "")
			{
				ret = ret.Substring(0, ret.Length - 2);
			}
			return ret;
		}

		public string getName()
		{
			if (Url.Contains("/"))
				return Url.Substring(Url.LastIndexOf("/") + 1, Url.Length);
			else
				return Url;
		}
	}
}
