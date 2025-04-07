using DegreeMapperWebAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DegreeMapper.PDFTemplate
{
    public class PDFTemplate
    {
        private int DegreeId { get; set; }

        public string HTMLPage { get; set; }
        public string PDFTitle { get; set; }
        public string PDFSubject { get; set; }
        public string PDFAuthor { get; set; } = "UCF Success Pathways";
        public string PDFFileName { get; set; }

        private const string PDFFooterImage = "https://portal.connect.ucf.edu/";

        private DegreeInfo DI { get; set; }

        private const string CriticalSpan = "<span class=\"badge badge-secondary p-1\">C</span>";
        private const string RequiredSpan = "<span class=\"badge badge-danger p-1\">R</span>";
        private const string CPPSpan = "<span class=\"badge badge-primary p-1\">CPP</span>";
        private const string Checkbox = "<input type=\"checkbox\" />";

        public PDFTemplate(int degreeId) 
        {
            DegreeId = degreeId;
            DI = DegreeInfo.Get(DegreeId);
            StringBuilder body = new StringBuilder();
            body.Append(GetPDFHeader());
            body.Append(GetRequirementsTable());
            body.Append(GetCourseMapperTable());
            body.Append(GetSemesterPathwaysTable());
            body.Append(GetDisclaimerTable());
            body.Append(GetFooterImage());
            GetHTMLPage(body.ToString());

            PDFTitle = $"{DI.CatalogYear} {DI.Degree} {DI.Institution}";
            PDFSubject = $"UCF Success Pathways Catalog";
            PDFFileName = $"{DI.CatalogYear}_{DI.Degree}_{DI.Institution}.pdf";
        }

        private void GetHTMLPage(string body)
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
            sb.Append($"{body}");
            sb.Append("</body>");
            sb.Append("</html>");
            HTMLPage =  sb.ToString();
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

        #region Get Course Mapper Table Section
        private string GetCourseMapperTable() {
            CourseMapper cm = new CourseMapper(DegreeId);

            #region Table Start
            StringBuilder sb = new StringBuilder();
            sb.Append($"<h2 class=\"bg-primary\">{DI.Institution}</h2>");
            sb.Append("<div class=\"container\">");
            sb.Append("<table class=\"table\">");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th>UCF Course Prefix & Course Number</th>");
            sb.Append("<th>Daytona State College Course Prefix & Number</th>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<th colspan=\"2\">");
            sb.Append("<div><span class=\"badge badge-secondary p-1\">C</span> Critical Course Requirements</div>");
            sb.Append("<div><span class=\"badge badge-danger p - 1\">R</span> Required Course</div>");
            sb.Append("<div><span class=\"badge badge-primary p-1\">CPP</span> Common Program Prerequisite</div>");
            sb.Append("</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            #endregion

            #region Primary Courses
            if (cm.UCFCourses.Count > 0 || cm.PartnerCourses.Count > 0)
            {
                sb.Append("<tr>");
                sb.Append(GetUCFCourseMapperTD(cm.UCFCourses, string.Empty));
                sb.Append(GetPartnerCourseMapperTD(cm.PartnerCourses, string.Empty));
                sb.Append("</tr>");
            }
            #endregion

            #region Alternate Courses
            if (cm.AlternateUCFCourses.Count > 0 || cm.AlternatePartnerCourses.Count > 0)
            {
                sb.Append("<tr>");
                sb.Append(GetUCFCourseMapperTD(cm.AlternateUCFCourses, cm.DisplayName));
                sb.Append(GetPartnerCourseMapperTD(cm.AlternatePartnerCourses, cm.AlternateDisplayName));
                sb.Append("</tr>");
            }
            #endregion

            #region Alternate 2 Courses
            if (cm.Alternate2UCFCourses.Count > 0 || cm.Alternate2PartnerCourses.Count > 0)
            {
                sb.Append("<tr>");
                sb.Append(GetUCFCourseMapperTD(cm.Alternate2UCFCourses, cm.Alternate2DisplayName));
                sb.Append(GetPartnerCourseMapperTD(cm.Alternate2PartnerCourses, cm.Alternate2DisplayName));
                sb.Append("</tr>");
            }
            #endregion

            #region Alternate 3 Courses
            if (cm.Alternate3UCFCourses.Count > 0 || cm.Alternate3PartnerCourses.Count > 0)
            {
                sb.Append("<tr>");
                sb.Append(GetUCFCourseMapperTD(cm.Alternate3UCFCourses, cm.Alternate3DisplayName));
                sb.Append(GetPartnerCourseMapperTD(cm.Alternate3PartnerCourses, cm.Alternate3DisplayName));
                sb.Append("</tr>");
            }
            #endregion

            #region Alternate 4 Courses
            if (cm.Alternate4UCFCourses.Count > 0 || cm.Alternate4PartnerCourses.Count > 0)
            {
                sb.Append("<tr>");
                sb.Append(GetUCFCourseMapperTD(cm.Alternate4UCFCourses, cm.Alternate4DisplayName));
                sb.Append(GetPartnerCourseMapperTD(cm.Alternate4PartnerCourses, cm.Alternate4DisplayName));
                sb.Append("</tr>");
            }
            #endregion

            #region Alternate 5 Courses
            if (cm.Alternate5UCFCourses.Count > 0 || cm.Alternate5PartnerCourses.Count > 0)
            {
                sb.Append("<tr>");
                sb.Append(GetUCFCourseMapperTD(cm.Alternate5UCFCourses, cm.Alternate5DisplayName));
                sb.Append(GetPartnerCourseMapperTD(cm.Alternate5PartnerCourses, cm.Alternate5DisplayName));
                sb.Append("</tr>");
            }
            #endregion

            #region Table End
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("</div>");
            #endregion

            return sb.ToString();
        }

        private string GetUCFCourseMapperTD(List<Course> courseList, string displayName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<td>");
            if (!string.IsNullOrEmpty(displayName))
            {
                sb.Append($"<div class=\"row\"><div class=\"col-md-12\">{displayName}</div></div>");
            }
            foreach (Course course in courseList)
            {
                sb.Append("<div class=\"row\">");
                sb.Append("div class\"col-md-2\"");
                if (course.Critical)
                {
                    sb.Append(CriticalSpan);
                }
                if (course.Required)
                {
                    sb.Append(RequiredSpan);
                }
                if (course.CommonProgramPrerequiste)
                {
                    sb.Append(CPPSpan);
                }
                sb.Append("</div>");
                sb.Append($"<div class=\"col-md-7\">{Checkbox} {course.Name}</div>");
                sb.Append($"<div> class=\"col-md-3\"> {course.Credits} Credits</div>");
                sb.Append("</div>");
            }
            sb.Append("</td>");
            return sb.ToString();
        }

        private string GetPartnerCourseMapperTD(List<Course> courseList, string displayName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<td>");
            if (!string.IsNullOrEmpty(displayName))
            {
                sb.Append($"<div class=\"row\"><div class=\"col-md-12\">{displayName}</div></div>");
            }
            foreach (Course pcourse in courseList)
            {
                sb.Append("<div class=\"row\">");
                sb.Append($"<div class=\"col-md-8\"{pcourse.Name}</div>");
                sb.Append($"<div class=\"col-md-4\"{pcourse.Credits}</div>");
                sb.Append("</div>");
            }
            sb.Append("</td>");
            return sb.ToString();
        }
        #endregion

        private string GetSemesterPathwaysTable()
        {
            StringBuilder sb = new StringBuilder();
            List<CustomCourseMapper> list = CustomCourseMapper.List(DegreeId);

            string semesterTerm = string.Empty;
            foreach (CustomCourseMapper ccm in list.OrderBy(x=>x.Semester).ThenBy(x=>x.TermOrder))
            {
                sb.Append("<table class=\"table\">");
                if (string.IsNullOrEmpty(semesterTerm) || semesterTerm.ToLower() != ccm.SemesterTerm.ToLower())
                {
                    semesterTerm = ccm.SemesterTerm;
                    sb.Append($"<tr><th colspan=\"2\" class=\"bg-primary\"><h4>{semesterTerm}</h4></th></tr>");
                    sb.Append($"<tr><td><strong>Course</strong></td><td class=\"text-right\"><strong>Credits</strong></td></tr>");
                }                
                sb.Append($"<tr><td>{Checkbox} <strong>{ccm.Course}</strong></td><td class=\"text-right\">{ccm.Credit}</td></tr>");
                sb.Append("</table>");
            }
            return sb.ToString();
        }

        private string GetDisclaimerTable()
        {
            string disclaimer = @"Success Pathways do not substitute for your advisor, degree planning tools, and degree audits. Once you enroll at UCF, Pegasus Path (degree planning) and myKnight Audit (degree audit) are the official tools at UCF. Please choose your major of choice early and follow Success Pathways in consultation with your advisor. Actual degree requirements at each institution are based on the undergraduate catalog year in which you first enrolled in the institution.";
            return $"<div><strong>Disclaimer</strong></div><div>{disclaimer}</div>";
        }

        private string GetFooterImage()
        { 
            StringBuilder sb = new StringBuilder();
            sb.Append($"<div><img src=\"{ PDFFooterImage}\"/></div>");
            return sb.ToString();
        }
    }
}
