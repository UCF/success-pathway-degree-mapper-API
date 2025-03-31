using DegreeMapperWebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegreeMapper.PDFTemplate
{
    public class PDFTemplate
    {
        private int DegreeId { get; set; }

        private string Body { get; set; }
        private string PDFHeader { get; set; }

        private DegreeInfo DI { get; set; }

        public PDFTemplate(int degreeId) 
        {
            DegreeId = degreeId;
            Body = GetHTMLPage();
            Body += GetPDFHeader();
            Body += GetRequirementsTable();

            
        }

        private string GetHTMLPage()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html>");
            sb.Append("<html lang=\"en\">");
            sb.Append("<head>");
            sb.Append("<meta charset=\"UTF-8\">");
            sb.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale= 1.0\">");
            sb.Append("<title>UCF Success Pathways</title>");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append($"{Body}");
            sb.Append("</body>");
            sb.Append("</html>");



            return sb.ToString();
        }

        private string GetPDFHeader()
        { 
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='text-center'>");
            sb.Append("<h1>UCF Success Pathways</h1>");
            sb.Append($"<h2>{DI.CatalogYear} {DI.Degree}</h2>");
            sb.Append($"<h3>{DI.Institution}</h3");
            sb.Append("</div>");
            return sb.ToString();
        }

        private string GetRequirementsTable()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table class=\"table\">");
            sb.Append("<tbody>");
            sb.Append("<tr>");
            sb.Append("<td><strong>Required GPA:</strong></td>");
            sb.Append($"<td>{DI.GPA}</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td><strong>Limited Access:</strong></td>");
            sb.Append($"<td>{DI.LimitedAccess.ToString()}</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td><strong>Restricted Access:</strong></td>");
            sb.Append($"<td>{DI.RestrictedAccess.ToString()}</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td><strong>Foreign Language Requirements:</strong></td>");
            sb.Append($"<td>{DI.ForeignLanguageRequirement}</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td><strong>Additional Requirements :</strong></td>");
            sb.Append($"<td>{DI.AdditionalRequirement}</td>");
            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");
            return sb.ToString();

        }

        private string GetCourseMapperTable() {
            CourseMapper cm = new CourseMapper(DegreeId);
            cm.UCFCourses



            return string.Empty;
        }

    }
}
