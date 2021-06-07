using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using hefesto.base_hefesto.Services;
using Microsoft.AspNetCore.Mvc;
using hefesto.base_hefesto.Models;

namespace hefesto.base_hefesto.Report
{
    public class BaseViewReportController : Controller
	{
        //public string RepType { get; }

		private readonly IMessageService _messageService;

		public BaseViewReportController(IMessageService messageService)
        {
			this._messageService = messageService;
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
					igrupo = this._messageService.GetMessage("reportTypeGroups.docs");
				if (grupo.Equals(ReportType.Groups()[1]))
					igrupo = this._messageService.GetMessage("reportTypeGroups.sheets");
				if (grupo.Equals(ReportType.Groups()[2]))
					igrupo = this._messageService.GetMessage("reportTypeGroups.text");
				if (grupo.Equals(ReportType.Groups()[3]))
					igrupo = this._messageService.GetMessage("reportTypeGroups.others");

				grupoVO = new ReportGroupVO(igrupo, subtipos);
				listaVO.Add(grupoVO);
			}

			return listaVO;
		}

		protected void LoadMessages()
		{
			ViewData["AlertMessage"] = new AlertMessageVO();
			ViewData["ListReportType"] = this.GetListReportType();

			foreach (var msg in _messageService.GetMessages())
			{
				ViewData[msg.Key] = msg.Value;
			}
		}

	}
}
