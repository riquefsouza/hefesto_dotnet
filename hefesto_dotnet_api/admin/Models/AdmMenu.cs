using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace hefesto.admin.Models
{
    public partial class AdmMenu
    {
        public AdmMenu()
        {
            InverseAdmMenuParent = new HashSet<AdmMenu>();
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public long? IdMenuParent { get; set; }
        public long? IdPage { get; set; }
        public int? Order { get; set; }

        public virtual AdmPage AdmPage { get; set; }
        public virtual AdmMenu AdmMenuParent { get; set; }

        [JsonIgnore]
        public virtual ICollection<AdmMenu> InverseAdmMenuParent { get; set; }
        
        [JsonIgnore]
        public virtual string Url { get; set; }
    }
}
