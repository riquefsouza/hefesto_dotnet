using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#nullable disable

namespace hefesto.admin.Models
{
    public partial class AdmParameter
    {
        public long Id { get; set; }
        public string Code { get; set; }

        [Required(ErrorMessage = "The field Description is required")]
        public string Description { get; set; }

        [JsonIgnore]        
        public long IdParameterCategory { get; set; }
        public string Value { get; set; }

        public virtual AdmParameterCategory AdmParameterCategory { get; set; }
    }
}
