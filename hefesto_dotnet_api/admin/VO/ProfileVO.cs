using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.admin.VO
{
    public class ProfileVO
    {
		public long Id { get; set; }

		public bool? Administrator { get; set; }

		public string Description { get; set; }

		public bool? General { get; set; }

		public bool Intersection { get; set; }

		public List<PageVO> Pages { get; set; }

		public List<UserVO> Users { get; set; }

		public List<UserVO> ExcludedUsers { get; set; }

		public ProfileVO()
		{
			this.Pages = new List<PageVO>();
			this.Users = new List<UserVO>();
			this.ExcludedUsers = new List<UserVO>();
			Clean();
		}

		public void Clean()
		{
			this.Id = -1;
			this.Administrator = false;
			this.Description = null;
			this.General = false;
			this.Intersection = false;
			this.Pages.Clear();
			this.Users.Clear();
			this.ExcludedUsers.Clear();
		}

		override public string ToString()
		{
			return Description;
		}
	}
}
