using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable disable

namespace hefesto.admin.Models
{
    public partial class AdmPage
    {
        public AdmPage()
        {
            AdmMenus = new HashSet<AdmMenu>();
            AdmPageProfiles = new HashSet<AdmPageProfile>();
            AdmIdProfiles = new HashSet<long>();
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

        [JsonIgnore]
        public virtual ICollection<AdmMenu> AdmMenus { get; set; }

        [JsonIgnore]
        public virtual ICollection<AdmPageProfile> AdmPageProfiles { get; set; }
        
        [NotMapped]
        public ICollection<long> AdmIdProfiles { get; set; }

        [NotMapped]
        public string PageProfiles { get; set; }

    }
}
