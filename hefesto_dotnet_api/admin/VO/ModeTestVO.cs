using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.admin.VO
{
    public class ModeTestVO : IEquatable<ModeTestVO>
	{
		public bool? Active { get; set; }

		public string Login { get; set; }

		public string LoginVirtual { get; set; }

		public ModeTestVO()
		{
			this.Active = false;
			this.Login = "";
			this.LoginVirtual = "";
		}

		public ModeTestVO(bool active, string login, string loginVirtual)
		{
			this.Active = active;
			this.Login = login;
			this.LoginVirtual = loginVirtual;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ModeTestVO);
		}

		public bool Equals(ModeTestVO other)
		{
			if (other == null)
				return false;

			if (this.GetType() != other.GetType())
				return false;

			if (Active == null)
			{
				if (other.Active != null)
					return false;
			}
			else if (!Active.Equals(other.Active))
				return false;

			if (Login == null)
			{
				if (other.Login != null)
					return false;
			}
			else if (!Login.Equals(other.Login))
				return false;

			if (LoginVirtual == null)
			{
				if (other.LoginVirtual != null)
					return false;
			}
			else if (!LoginVirtual.Equals(other.LoginVirtual))
				return false;

			return true;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Active, Login, LoginVirtual);
		}

		override public string ToString()
		{
			return "ModeTestVO [active=" + Active + ", login=" + Login + ", loginVirtual=" + LoginVirtual + "]";
		}
	}
}
