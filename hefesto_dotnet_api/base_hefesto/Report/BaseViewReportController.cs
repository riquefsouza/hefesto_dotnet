using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using hefesto.base_hefesto.Services;
using Microsoft.AspNetCore.Mvc;
using hefesto.admin.Services;

namespace hefesto.base_hefesto.Report
{
    public class BaseViewReportController : BaseController
	{
        //public string RepType { get; }

		public BaseViewReportController(IMessageService messageService,
			ISystemService systemService) : base(messageService, systemService)
        {
			
			//this.RepType = ReportType.GetReportType(ReportTypeEnum.PDF).FileExtension.ToUpper();
		}

		public List<ReportGroupVO> GetListReportType()
		{
			List<ReportGroupVO> listaVO = new List<ReportGroupVO>();
			ReportGroupVO grupoVO;
			List<ReportType> listaEnum = ReportType.AllTypes();
			List<ReportType> subtipos = new List<ReportType>();

			foreach (var grupo in ReportType.Groups())
			{
				string igrupo = "";

				subtipos = listaEnum
					.Where(item => item.Group.Equals(grupo))
					.ToList();

				if (grupo.Equals(ReportType.Groups()[0]))
					igrupo = this.MessageService.GetMessage("reportTypeGroups.docs");
				if (grupo.Equals(ReportType.Groups()[1]))
					igrupo = this.MessageService.GetMessage("reportTypeGroups.sheets");
				if (grupo.Equals(ReportType.Groups()[2]))
					igrupo = this.MessageService.GetMessage("reportTypeGroups.text");
				if (grupo.Equals(ReportType.Groups()[3]))
					igrupo = this.MessageService.GetMessage("reportTypeGroups.others");

				grupoVO = new ReportGroupVO(igrupo, subtipos);
				listaVO.Add(grupoVO);
			}

			return listaVO;
		}

		override protected void LoadMessages()
		{
			base.LoadMessages();
			ViewData["ListReportType"] = this.GetListReportType();
		}

	}
}
