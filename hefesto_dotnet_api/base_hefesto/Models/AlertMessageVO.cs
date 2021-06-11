using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hefesto.base_hefesto.Services;

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

		public static AlertMessageVO Primary(IMessageService messageService, string key)
		{
			AlertMessageVO vo = new AlertMessageVO();
			vo.PrimaryMessage = messageService.GetMessage(key);
			return vo;
		}

		public static AlertMessageVO Secondary(IMessageService messageService, string key)
		{
			AlertMessageVO vo = new AlertMessageVO();
			vo.SecondaryMessage = messageService.GetMessage(key);
			return vo;
		}

		public static AlertMessageVO Success(IMessageService messageService, string key)
		{
			AlertMessageVO vo = new AlertMessageVO();
			vo.SuccessMessage = messageService.GetMessage(key);
			return vo;
		}

		public static AlertMessageVO Danger(IMessageService messageService, string key)
		{
			AlertMessageVO vo = new AlertMessageVO();
			vo.DangerMessage = messageService.GetMessage(key);
			return vo;
		}

		public static AlertMessageVO Warning(IMessageService messageService, string key)
		{
			AlertMessageVO vo = new AlertMessageVO();
			vo.WarningMessage = messageService.GetMessage(key);
			return vo;
		}

		public static AlertMessageVO Info(IMessageService messageService, string key)
		{
			AlertMessageVO vo = new AlertMessageVO();
			vo.InfoMessage = messageService.GetMessage(key);
			return vo;
		}

		public static AlertMessageVO Light(IMessageService messageService, string key)
		{
			AlertMessageVO vo = new AlertMessageVO();
			vo.LightMessage = messageService.GetMessage(key);
			return vo;
		}

		public static AlertMessageVO Dark(IMessageService messageService, string key)
		{
			AlertMessageVO vo = new AlertMessageVO();
			vo.DarkMessage = messageService.GetMessage(key);
			return vo;
		}

	}
}
