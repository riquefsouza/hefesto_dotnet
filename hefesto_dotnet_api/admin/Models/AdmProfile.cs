using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable disable

namespace hefesto.admin.Models
{
    public partial class AdmProfile
    {
        public AdmProfile()
        {
            AdmPageProfiles = new HashSet<AdmPageProfile>();
            AdmUserProfiles = new HashSet<AdmUserProfile>();
            AdmPages = new HashSet<AdmPage>();
            AdmUsers = new HashSet<AdmUser>();
        }

        public long Id { get; set; }
        public char? Administrator { get; set; }
        public string Description { get; set; }
        public char? General { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<AdmPageProfile> AdmPageProfiles { get; set; }

        [JsonIgnore]        
        public virtual ICollection<AdmUserProfile> AdmUserProfiles { get; set; }
        
        [NotMapped]
        public ICollection<AdmPage> AdmPages { get; set; }

        [NotMapped]	
        public ICollection<AdmUser> AdmUsers { get; set; }

        [NotMapped]        
        public string ProfilePages { get; set; }

        [NotMapped]        
        public string ProfileUsers { get; set; }

    }
}
