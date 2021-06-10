using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.admin.VO
{
    public class UserVO
    {
		public long Id { get; set; }

		public string Ip { get; set; }

		public DateTime Date { get; set; }

		public string Email { get; set; }

		public string Login { get; set; }

		public string Name { get; set; }

		public string Password { get; set; }

		public bool Active { get; set; }

		public List<long> AdmIdProfiles { get; set; }

		public string UserProfiles { get; set; }

		public string CurrentPassword { get; set; }

		public string NewPassword { get; set; }

		public string ConfirmNewPassword { get; set; }

		public UserVO()
		{
			Clean();
		}

		public UserVO(long id, string email, string login, string name, bool active)
		{
			this.Id = id;
			this.Email = email;
			this.Login = login;
			this.Name = name;
			this.Active = active;
		}

		public void Clean()
		{
			this.Ip = "";
			this.Date = DateTime.Now;
			this.Email = "";
			this.Login = "";
			this.Name = "";
			this.Password = "";
			this.Active = false;
			this.AdmIdProfiles = new List<long>();
			this.UserProfiles = "";
			this.CurrentPassword = "";
			this.NewPassword = "";
			this.ConfirmNewPassword = "";
		}
	}
}
