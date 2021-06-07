using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.base_hefesto.Report
{
    public class ReportGroupVO
    {
        public string Group { get; set; }

		public List<ReportType> Types { get; set; }
	
		public ReportGroupVO(): base()
		{
		}

		public ReportGroupVO(string group, List<ReportType> types)
		{
			this.Group = group;
			this.Types = types;
		}

	}
}
