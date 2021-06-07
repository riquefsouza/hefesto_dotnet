using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#nullable disable

namespace hefesto.admin.Models
{
    public partial class AdmUser
    {
        public AdmUser()
        {
            AdmUserProfiles = new HashSet<AdmUserProfile>();
            AdmIdProfiles = new HashSet<long>();
        }

        public long Id { get; set; }
        public char? Active { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "The field Login is required")]
        public string Login { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        [JsonIgnore]
        public virtual ICollection<AdmUserProfile> AdmUserProfiles { get; set; }

        [NotMapped]
        public ICollection<long> AdmIdProfiles { get; set; }

        [NotMapped]        
        public string UserProfiles { get; set; }

        [NotMapped]        
        public string CurrentPassword { get; set; }

        [NotMapped]        
        public string NewPassword { get; set; }

        [NotMapped]        
        public string ConfirmNewPassword { get; set; }


    }
}
