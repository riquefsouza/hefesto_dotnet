using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using hefesto.admin.VO;

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

        /*
        [JsonIgnore]
        public char AdministratorChar { get; set; }

        [NotMapped]
        public bool Administrator
        {
            get
            {
                if (AdministratorChar.Equals('S'))
                    return true;
                else
                    return false;
            }

            set
            {
                if (this.Administrator == true)
                    AdministratorChar = 'S';
                else
                    AdministratorChar = 'N';
            }
        }
        */
        public bool? Administrator { get; set; }

        [Required(ErrorMessage = "The field Description is required")]
        public string Description { get; set; }

        /*
        [JsonIgnore]
        public char GeneralChar { get; set; }

        [NotMapped]
        public bool General
        {
            get
            {
                if (GeneralChar.Equals('S'))
                    return true;
                else
                    return false;
            }

            set
            {
                if (this.General == true)
                    GeneralChar = 'S';
                else
                    GeneralChar = 'N';
            }
        }
        */
        public bool? General { get; set; }

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

        public ProfileVO ToProfileVO()
        {
            ProfileVO p = new ProfileVO();

            p.Id = this.Id;
            p.Administrator = this.Administrator;
            p.Description = this.Description;
            p.General = this.General;

            foreach (AdmPage admPagina in this.AdmPages)
            {
                p.Pages.Add(admPagina.ToPageVO());
            }

            foreach (AdmUser admUser in this.AdmUsers)
            {
                p.Users.Add(admUser.ToUserVO());
            }

            return p;
        }
    }
}
