using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace hefesto.admin.Models
{
    public partial class AdmParameterCategory
    {
        public AdmParameterCategory()
        {
            AdmParameters = new HashSet<AdmParameter>();
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public long? Order { get; set; }

         [JsonIgnore]
        public virtual ICollection<AdmParameter> AdmParameters { get; set; }
    }
}
