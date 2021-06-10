using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using hefesto.admin.VO;

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

        [Required(ErrorMessage = "The field Description is required")]
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

        public String PerfisPage()
        {
            String ret = "";
            foreach (AdmPageProfile item in this.AdmPageProfiles)
            {
                ret = ret + item.AdmProfile.Description + ", ";
            }
            if (ret != "")
            {
                ret = ret.Substring(0, ret.Length - 2);
            }
            return ret;
        }

        public PageVO ToPageVO()
        {
            PageVO p = new PageVO();

            p.Id = this.Id;
            p.Description = this.Description;
            p.Url = this.Url;

            return p;
        }
    }
}
