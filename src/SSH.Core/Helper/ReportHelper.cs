using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using SSH.Core.Constant;
using SSH.Core.Infrastructure;

namespace SSH.Core.Helper
{
    public class ReportHelper
    {
        public static string CreateCSVFile<T>(List<T> data, string reportName)
        {
            string folderPath = HttpContext.Current.ApplicationInstance.Server.MapPath("~/Reports") + "\\CSVReports";

            if (!Directory.Exists(folderPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(folderPath);
            }

            string filename = reportName + "_" + DateTime.Now.ToString(Validations.DateTimeFormate) + ".csv";
            string filePath = folderPath + "\\" + filename;

            ReportGenerator.CreateCSV(data, filePath);
            filePath = string.Format(@"{0}\{1}\{2}", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), "\\Reports\\CSVReports\\", filename);

            return filePath;
        }

        public static string CreatePDFFile<T>(List<T> data, string reportName)
        {
            string folderPath = HttpContext.Current.ApplicationInstance.Server.MapPath("~/Reports") + "\\PDFReports";

            if (!Directory.Exists(folderPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(folderPath);
            }

            string fileCreationTime = reportName + "_" + DateTime.Now.ToString(Validations.DateTimeFormate) + ".pdf";
            string filename = folderPath + @"\" + fileCreationTime;

            ReportGenerator.CreatePDF(data, folderPath, filename);
            string filePath = string.Format(@"{0}\{1}\{2}", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), "\\Reports\\PDFReports\\", fileCreationTime);

            return filePath;
        }
    }
}
