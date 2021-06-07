using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.base_hefesto.Models
{
    public class AlertMessageVO
    {
        public string PrimaryMessage { get; set; }
        public string SecondaryMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string DangerMessage { get; set; }
        public string WarningMessage { get; set; }
        public string InfoMessage { get; set; }
        public string LightMessage { get; set; }
        public string DarkMessage { get; set; }

        public AlertMessageVO()
        {
            this.PrimaryMessage = "";
            this.SecondaryMessage = "";
            this.SuccessMessage = "";
            this.DangerMessage = "";
            this.WarningMessage = "";
            this.InfoMessage = "";
            this.LightMessage = "";
            this.DarkMessage = "";
        }
    }
}
