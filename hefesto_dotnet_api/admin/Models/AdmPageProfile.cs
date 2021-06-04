using System;
using System.Collections.Generic;

#nullable disable

namespace hefesto.admin.Models
{
    public partial class AdmPageProfile
    {
        public long Id { get; set; }
        public long IdProfile { get; set; }
        public long IdPage { get; set; }

        public virtual AdmPage AdmPage { get; set; }
        public virtual AdmProfile AdmProfile { get; set; }
    }
}
