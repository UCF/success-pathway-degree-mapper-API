﻿using DegreeMapperWebAPI;
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
            sb.Append("<link href=\"https://connect.ucf.edu/wp-content/themes/Colleges-Theme/static/css/style.min.css?ver=6.7.1\" rel=\"stylesheet\" media=\"all\" />");
            sb.Append("</head>");
            sb.Append("<body style=\"margin: 50px\">");
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

                foreach (CourseMapper cm in list_cm.OrderBy(x => x.SortOrder))
                {
                    sb.Append("<tr>");
                    #region UCF Courses
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
                        sb.Append($"<div>{displayCheckbox}{critical}{required}{cpp} {c.Code} {credit} credits</div>");
                        sb.Append("</div>");
                    }
                }
            }
            return sb.ToString();
        }


        private string GetUCFCourseMapperTD_OLD(List<Course> courseList, string displayName)
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
                sb.Append($"<div class=\"col-md-7 text-left\">{course.Name}</div>");
                sb.Append($"<div> class=\"col-md-3 text-right\"> {course.Credits} Credits</div>");
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

            string semesterTerm = string.Empty;
            if (list.Count > 0)
            {
                sb.Append("<div style=\"page-break-before: always\"></div>");
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
                            $"<td><strong>Course</strong></td>" +
                            $"<td class=\"text-right\"><strong>Credits</strong></td>" +
                            $"</tr>");
                    }
                    sb.Append($"<tr>" +
                        $"<td class=\"text-left\"><strong>{ccm.Course}</strong></td>" +
                        $"<td class=\"text-right\">{ccm.Credit}</td></tr>");
                    sb.Append("</table>");
                }
            }
            sb.Append("<div style=\"page-break-before: always\"></div>");
            return sb.ToString();
        }

        private string GetDisclaimerTable()
        {
            string disclaimer = @"Success Pathways do not substitute for your advisor, degree planning tools, and degree audits. Once you enroll at UCF, Pegasus Path (degree planning) and myKnight Audit (degree audit) are the official tools at UCF. Please choose your major of choice early and follow Success Pathways in consultation with your advisor. Actual degree requirements at each institution are based on the undergraduate catalog year in which you first enrolled in the institution.";
            return $"<div><strong>Disclaimer</strong></div><div>{disclaimer}</div>";
        }

        private string GetCourseMapperDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"text-left h5\">Listed courses are to account for degree requirements, pre-requisites and critical courses to prepare students for admission to intended major at UCF.Courses should be completed before transferring to UCF.</div>");
            sb.Append("<div class=\"text-left h5 pb-2\">This program may have different tracks.Please refer to the UCF Catalog for additional coursework you can complete while earning your AA.</div>");
            return sb.ToString();
        }

        private string GetCustomCourseMapperDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"text-left pt-3 h5\">The semester curriculum suggested below assumes that the student: a) has earned an A.A., b) has completed required lower-level courses and academic milestones, c) will be enrolled full-time at UCF. Upon matriculation to UCF, the student is strongly encouraged to consult their major advisor at UCF and utilize the academic planning tools such as Pegasus Path, myKnight Audit, and mySchedule Builder for any of their academic planning needs.</div>");
            return sb.ToString();
        }
        private string GetFooterImage()
        { 
            StringBuilder sb = new StringBuilder();
            sb.Append($"<div><img src=\"{ PDFFooterImage}\"/></div>");
            return sb.ToString();
        }
    }
}
