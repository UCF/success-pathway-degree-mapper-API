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

        private const string PDFHeaderImage = "https://dev.portal.connect.ucf.edu/pathway/images/PDFHeaderImage.png";

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
            body.Append(GetHeaderImage());
            body.Append(GetPDFHeader());
            body.Append(GetRequirementsTable());
            body.Append(GetCourseMapperTable());
            body.Append(GetSemesterPathwaysTable());
            body.Append(GetDisclaimerTable());
            GetHTMLPage(body.ToString());

            PDFTitle = $"{DI.CatalogYear} {DI.Degree} {DI.Institution}";
            PDFSubject = $"UCF Success Pathways Catalog";
            //PDFFileName = $"{DI.CatalogYear}_{DI.Degree}_{DI.Institution}.pdf";
            PDFFileName = "SuccessPathwayDegree.pdf";
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
            sb.Append("<link rel=\"stylesheet\" href=\"https://dev.portal.connect.ucf.edu/pathway/content/bootstrap-theme.min.css\" />");
            //sb.Append("<link href=\"https://connect.ucf.edu/wp-content/themes/Colleges-Theme/static/css/style.min.css?ver=6.7.1\" rel=\"stylesheet\" media=\"all\" />");
            sb.Append(GetCustomCss());
            sb.Append("</head>");
            sb.Append("<body style=\"margin: 50px\">");
            sb.Append($"{body}");
            sb.Append("</body>");
            sb.Append("</html>");
            HTMLPage =  sb.ToString();
        }

        private string GetCustomCss()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<style>");
            sb.Append(".bg-primary, .badge-primary { background-color: #fc0 !important; color:black }");
            sb.Append(".badge-secondary { background-color: black; color:white }");
            sb.Append("</style>");
            return sb.ToString();
        }

        private string GetPDFHeader()
        { 
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"text-center pb-4\">");
            sb.Append("<h1>UCF Success Pathways</h1>");
            sb.Append($"<h2>{DI.CatalogYear} {DI.Degree}</h2>");
            sb.Append($"<h3>State College Plan: {DI.Institution} Plan</h3");
            sb.Append("</div>");
            sb.Append("<p>&nbsp;</p>");
            return sb.ToString();
        }

        private string GetRequirementsTable()
        {
            //@(Model.LimitedAccess?"Yes":"No")
            StringBuilder sb = new StringBuilder();
            sb.Append("<table class=\"table pt-5 h5\"");
            sb.Append("<tbody>");
            sb.Append("<tr style=\"padding-top:25px\">");
            sb.Append($"<td><strong>Required GPA:</strong> {DI.GPA}</td>");
            sb.Append($"<td><strong>Limited Access:</strong> {(DI.LimitedAccess ? "Yes" : "No")}</td>");
            sb.Append($"<td><strong>Restricted Access:</strong> {(DI.RestrictedAccess ? "Yes" : "No")}</td>");
            sb.Append("</tr>");
            sb.Append("</body>");
            sb.Append("</table>");

            sb.Append("<table class=\"table pt-5 h5\"");
            sb.Append("<tbody>");
            sb.Append("<tr>");
            sb.Append("<th><strong>Foreign Language Requirements:</strong></th>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append($"<td class=\"text-left\">{DI.ForeignLanguageRequirement}</td>");
            sb.Append("</tr>");
            sb.Append("</body>");
            sb.Append("</table>");

            sb.Append("<table class=\"table pt-5 h5\"");
            sb.Append("<tbody>");
            sb.Append("<tr>");
            sb.Append("<th><strong>Additional Requirements:</strong></th>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append($"<td class=\"text-left\">{DI.AdditionalRequirement}</td>");
            sb.Append("</tr>");
            sb.Append("</body>");
            sb.Append("</table>");

            sb.Append("</tbody>");
            sb.Append("</table>");
            return sb.ToString();

        }

        #region Get Course Mapper Table Section
        private string GetCourseMapperTable() {
            //CourseMapper cm = new CourseMapper(DegreeId);
            List<CourseMapper> list_cm = CourseMapper.List(DegreeId, null, null);
            StringBuilder sb = new StringBuilder();
            sb.Append(GetCourseMapperDescription());
            if (list_cm.Count > 0)
            {
                #region Table Start
                //sb.Append("<div style=\"page-break-before: always\"></div>");
                sb.Append("<div class=\"container\">");
                sb.Append("<table class=\"table text-left h5\">");
                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th colspan=\"2\">UCF Course Prefix & Course Number</th>");
                sb.Append($"<th>{DI.Institution} Course Prefix & Number</th>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<th colspan=\"3\">");
                sb.Append("<div><span class=\"badge badge-secondary p-1\">C</span> Critical Course Requirements</div>");
                sb.Append("<div><span class=\"badge badge-danger p-1\">R</span> Required Course</div>");
                sb.Append("<div><span class=\"badge badge-primary p-1\">CPP</span> Common Program Prerequisite</div>");
                sb.Append("</th>");
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");
                #endregion

                foreach (CourseMapper cm in list_cm.OrderBy(x => x.SortOrder))
                {
                    sb.Append("<tr>");
                    #region UCF Courses
                    sb.Append($"<td>{Checkbox}</td>");
                    sb.Append("<td>");
                    sb.Append(SetCourseMapperDiv(cm.DisplayName, cm.UCFCourses, true));
                    sb.Append(SetCourseMapperDiv(cm.AlternateDisplayName, cm.AlternateUCFCourses, true));
                    sb.Append(SetCourseMapperDiv(cm.Alternate2DisplayName, cm.Alternate2UCFCourses, true));
                    sb.Append(SetCourseMapperDiv(cm.Alternate3DisplayName, cm.Alternate3UCFCourses, true));
                    sb.Append(SetCourseMapperDiv(cm.Alternate4DisplayName, cm.Alternate4UCFCourses, true));
                    sb.Append(SetCourseMapperDiv(cm.Alternate5DisplayName, cm.Alternate5UCFCourses, true));
                    #endregion
                    sb.Append("</td>");

                    #region Partner Courses
                    sb.Append("<td>");
                    sb.Append(SetCourseMapperDiv(cm.DisplayName, cm.PartnerCourses, false));
                    sb.Append(SetCourseMapperDiv(cm.AlternateDisplayName, cm.AlternatePartnerCourses, false));
                    sb.Append(SetCourseMapperDiv(cm.Alternate2DisplayName, cm.Alternate2PartnerCourses, false));
                    sb.Append(SetCourseMapperDiv(cm.Alternate3DisplayName, cm.Alternate3PartnerCourses, false));
                    sb.Append(SetCourseMapperDiv(cm.Alternate4DisplayName, cm.Alternate4PartnerCourses, false));
                    sb.Append(SetCourseMapperDiv(cm.Alternate5DisplayName, cm.Alternate5PartnerCourses, false));
                    #endregion
                    sb.Append("</td>");
                    sb.Append("</tr>");
                }

                #region Table End
                sb.Append("</tbody>");
                sb.Append("</table>");
                sb.Append("</div>");
                #endregion
            }
            return sb.ToString();
        }

        private string SetCourseMapperDiv(string title, List<Course> list_courses, bool useCheckbox)
        {
            StringBuilder sb = new StringBuilder();
            if(list_courses != null && list_courses.Count > 0)
            {
                string displayCheckbox = (useCheckbox ? Checkbox : string.Empty);

                if (title.ToLower() != "default")
                {
                    sb.Append($"{title}");
                }
                foreach (Course c in list_courses.OrderBy(x => x.Code))
                {
                    c.Code = (!string.IsNullOrEmpty(c.Description)) ? c.Code + "<br /><em>" + c.Description + "</em>" : c.Code;
                    string credit = (!string.IsNullOrEmpty(c.CreditText)) ? c.CreditText : c.Credits.ToString();
                    if (!string.IsNullOrEmpty(c.Code))
                    {
                        string critical = (c.Critical ? "<span class=\"badge badge-secondary p-1\">C</span>" : string.Empty);
                        string required = (c.Required ? "<span class=\"badge badge-danger p-1\">R</span>" : string.Empty);
                        string cpp = (c.CommonProgramPrerequiste ? "<span class=\"badge badge-primary p-1\">CPP</span>" : string.Empty);

                        sb.Append("<div class=\"pt-2\">");
                        sb.Append($"<div>{critical}{required}{cpp} {c.Code} {credit} credits</div>");
                        sb.Append("</div>");
                        //{displayCheckbox}
                    }
                }
            }
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
                sb.Append($"<div class=\"col-md-8\">{pcourse.Name}</div>");
                sb.Append($"<div class=\"col-md-4\">{pcourse.Credits}</div>");
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
            List<CustomCourseSemester> list2 = CustomCourseSemester.List(DegreeId, null);

            if (list2.Count > 0)
            {
                foreach (CustomCourseSemester ccs in list2)
                {
                    CustomCourseMapper cm = new CustomCourseMapper();
                    cm.Course = ccs.Note;
                    cm.Semester = ccs.Semester;
                    cm.Credit = string.Empty;
                    cm.SemesterTerm = (!string.IsNullOrEmpty(ccs.Term)) ? ccs.Semester.ToString() + " " + ccs.Term : ccs.Semester.ToString();
                    cm.TermOrder = "99";
                    list.Add(cm);
                }
            }

            string semesterTerm = string.Empty;
            if (list.Count > 0)
            {
                //sb.Append("<div style=\"page-break-before: always\"></div>");
                sb.Append(GetCustomCourseMapperDescription());
                foreach (CustomCourseMapper ccm in list.OrderBy(x => x.Semester).ThenBy(x => x.TermOrder))
                {
                    sb.Append("<table class=\"table h5\">");
                    if (string.IsNullOrEmpty(semesterTerm) || semesterTerm.ToLower() != ccm.SemesterTerm.ToLower())
                    {
                        semesterTerm = ccm.SemesterTerm;
                        sb.Append($"<tr>" +
                            $"<th colspan=\"2\" class=\"bg-primary text-left\">" +
                            $"<h4>Semester {semesterTerm}</h4>" +
                            $"</th>" +
                            $"</tr>");
                        sb.Append($"<tr>" +
                            $"<td class=\"text-left\"><strong>Course</strong></td>" +
                            $"<td class=\"text-right\"><strong>Credits</strong></td>" +
                            $"</tr>");
                    }
                    sb.Append($"<tr>" +
                        $"<td class=\"text-left\">{ccm.Course}</td>" +
                        $"<td class=\"text-right\">{ccm.Credit}</td></tr>");
                    sb.Append("</table>");
                }
            }
            //sb.Append("<div style=\"page-break-before: always\"></div>");
            return sb.ToString();
        }

        /// <summary>
        /// Returns plain text
        /// </summary>
        /// <returns></returns>
        private string GetDisclaimerTable()
        {
            string disclaimer = "Success Pathways do not substitute for your advisor, degree planning tools, and degree audits. Once you enroll at UCF, Pegasus Path (degree planning) and myKnight Audit (degree audit) are the official tools at UCF. Please choose your major of choice early and follow Success Pathways in consultation with your advisor. Actual degree requirements at each institution are based on the undergraduate catalog year in which you first enrolled in the institution.";
            return $"<div class\"text-left\"><strong>Disclaimer</strong></div><div class\"text-left\">{disclaimer}</div>";
        }

        /// <summary>
        /// Returns plain text
        /// </summary>
        /// <returns></returns>
        private string GetCourseMapperDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"text-left h5\">Listed courses are to account for degree requirements, pre-requisites and critical courses to prepare students for admission to intended major at UCF.Courses should be completed before transferring to UCF.</div>");
            sb.Append("<div class=\"text-left h5 pb-2\">This program may have different tracks.Please refer to the UCF Catalog for additional coursework you can complete while earning your AA.</div>");
            return sb.ToString();
        }

        /// <summary>
        /// Returns plain Text
        /// </summary>
        /// <returns></returns>
        private string GetCustomCourseMapperDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"text-left pt-3 h5\">The semester curriculum suggested below assumes that the student: a) has earned an A.A., b) has completed required lower-level courses and academic milestones, c) will be enrolled full-time at UCF. Upon matriculation to UCF, the student is strongly encouraged to consult their major advisor at UCF and utilize the academic planning tools such as Pegasus Path, myKnight Audit, and mySchedule Builder for any of their academic planning needs.</div>");
            return sb.ToString();
        }

        public string GetCustomCourseSemester(int degreeId)
        {
            StringBuilder sb = new StringBuilder();
            List<CustomCourseSemester> list = CustomCourseSemester.List(degreeId, null);
            return sb.ToString();
        }
        private string GetHeaderImage()
        { 
            StringBuilder sb = new StringBuilder();
            string img = $"<img src=\"{PDFHeaderImage}\" style=\"width:400px\"/>";
            sb.Append($"<div class=\"container text-center pb-4\">{img}</div>");
            return sb.ToString();
        }
    }
}
