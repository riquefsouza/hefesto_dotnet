using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using hefesto.admin.VO;

#nullable disable

namespace hefesto.admin.Models
{
    public partial class AdmUser
    {
        public long Id { get; set; }

        /*
        [JsonIgnore]
        public char ActiveChar { get; set; }

        [NotMapped]
        public bool Active { 
            get {
                if (ActiveChar.Equals('S'))
                    return true;
                else
                    return false;
            }

            set {
                if (this.Active == true)
                    ActiveChar = 'S';
                else
                    ActiveChar = 'N';
            } 
        }
        */
        public bool? Active { get; set; }

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

        public AdmUser()
        {
            AdmUserProfiles = new HashSet<AdmUserProfile>();
            AdmIdProfiles = new HashSet<long>();
            Active = false;
        }

        public AdmUser(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }

        public AdmUser(UserVO vo)
        {
            this.Id = vo.Id;
            this.Email = vo.Email;
            this.Login = vo.Login;
            this.Name = vo.Name;
            //this.Password = vo.Password;
            this.Active = vo.Active;
            this.AdmIdProfiles = vo.AdmIdProfiles;
            this.UserProfiles = vo.UserProfiles;
        }

        public UserVO ToUserVO()
        {
            UserVO u = new UserVO();

            u.Id = this.Id;
            //u.Ip = this.Ip;
            u.Email = this.Email;
            u.Login = this.Login;
            u.Name = this.Name;

            return u;
        }
    }
}
