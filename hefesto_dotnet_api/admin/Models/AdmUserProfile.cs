using System;
using System.Collections.Generic;

#nullable disable

namespace hefesto.admin.Models
{
    public partial class AdmUserProfile
    {
        public long Id { get; set; }
        public long IdProfile { get; set; }
        public long IdUser { get; set; }

        public virtual AdmProfile AdmProfile { get; set; }
        public virtual AdmUser AdmUser { get; set; }

        public AdmUserProfile(long IdUser, long IdProfile)
        {
            this.IdUser = IdUser;
            this.IdProfile = IdProfile;
        }
    }
}
