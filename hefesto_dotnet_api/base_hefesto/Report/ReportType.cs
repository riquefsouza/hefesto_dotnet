using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.base_hefesto.Report
{
    public class ReportType
    {
        public ReportTypeEnum Type { get; set; }

        public string Group { get; set; }

        public string ContentType { get; set; }

        public string Description { get; set; }

        public string FileExtension { get; set; }

        public ReportType(ReportTypeEnum type, string group, string contentType, string description, string fileExtension) 
        {
            this.Type = type;
            this.Group = group;
            this.ContentType = contentType;
            this.Description = description;
            this.FileExtension = fileExtension;
        }

        public static String[] Groups()
        {
            return new String[] { "Documentos", "Planilhas", "Texto puro", "Outros" };
        }

        public static List<ReportType> AllTypes()
        {
            List<ReportType> rt = new List<ReportType>();

            rt.Add(new ReportType(ReportTypeEnum.PDF, "Documentos", "application/pdf", "Portable Document Format (.pdf)", "pdf"));
            rt.Add(new ReportType(ReportTypeEnum.DOCX, "Documentos", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Microsoft Word XML (.docx)", "docx"));
            rt.Add(new ReportType(ReportTypeEnum.RTF, "Documentos", "application/rtf", "Rich Text Format (.rtf)", "rtf"));
            rt.Add(new ReportType(ReportTypeEnum.ODT, "Documentos", "application/vnd.oasis.opendocument.text", "OpenDocument Text (.odt)", "odt"));
            //rt.Add(new ReportType(ReportTypeEnum.XLS, "Planilhas", "application/vnd.ms-excel", "Microsoft Excel (.xls)"));
            rt.Add(new ReportType(ReportTypeEnum.XLSX, "Planilhas", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Microsoft Excel XML (.xlsx)", "xlsx"));
            rt.Add(new ReportType(ReportTypeEnum.ODS, "Planilhas", "application/vnd.oasis.opendocument.spreadsheet", "OpenDocument Spreadsheet (.ods)", "ods"));
            rt.Add(new ReportType(ReportTypeEnum.CSV, "Texto puro", "text/plain", "Valores Separados Por Vírgula (.csv)", "csv"));
            rt.Add(new ReportType(ReportTypeEnum.TXT, "Texto puro", "text/plain", "Somente Texto (.txt)", "txt"));
            rt.Add(new ReportType(ReportTypeEnum.PPTX, "Outros", "application/vnd.openxmlformats-officedocument.presentationml.presentation", "Microsoft Powerpoint XML (.pptx)", "pptx"));
            //rt.Add(new ReportType(ReportTypeEnum.HTML, "Outros", "text/html", "Linguagem de Marcação de Hipertexto (.html)"));
            rt.Add(new ReportType(ReportTypeEnum.HTML, "Outros", "application/zip", "Linguagem de Marcação de Hipertexto (.html)", "html"));

            return rt;
        }

        public static ReportType GetReportType(ReportTypeEnum type)
        {
            List<ReportType> rt = ReportType.AllTypes();
            return rt.Where(item => item.Type.Equals(type)).First();
        }

    }

}