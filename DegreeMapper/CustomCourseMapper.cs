using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DegreeMapperWebAPI
{
    public class CustomCourseMapper
    {
        public string Course { get; set; }
        public string Credit { get; set; }
        public int Semester { get; set; }
        public string SemesterTerm { get; set; }
        public string TermOrder { get; set; }
        public string Description { get; set; }

        public CustomCourseMapper()
        {

        }

        public CustomCourseMapper(Course c)
        {
            Course = c.Code;
            Credit = (!string.IsNullOrEmpty(c.CreditText)) ? c.CreditText : c.Credits.ToString();
            Semester = c.Semester;
            SemesterTerm = c.SemesterTerm;
            TermOrder = c.TermOrder;
            Description = c.Description;
        }

        public static List<CustomCourseMapper> List(int degreeId)
        {
            List<CustomCourseMapper> list_ccm = new List<CustomCourseMapper>();
            List<int> list_courseIds = new List<int>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetCustomCourseMapper";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@degreeId", degreeId);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //Updated stored procedure GetCustomCourseMapper and add DisplayMultipleSemesters 6/22/2025
                        bool displayMultipleSemesters = Convert.ToBoolean(dr["DisplayMultipleSemesters"].ToString());
                        list_courseIds = dr["CourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                        if (list_courseIds.Count > 0)
                        {
                            foreach (int id in list_courseIds)
                            {
                                Course c = DegreeMapperWebAPI.Course.Get(id);
                                CustomCourseMapper ccm = new CustomCourseMapper(c);
                                if (c.Active && c.Semester > 1)
                                {
                                    list_ccm.Add(ccm);
                                }
                            }
                        }
                    }
                }
                cn.Close();
            }
            if (list_ccm.Count == 0 && degreeId > 0)
            {
                GetUCFDefaultSemesterCourses(ref list_ccm, degreeId);
            }
            return list_ccm;
        }

        //gets UCF Default Courses
        public static void GetUCFDefaultSemesterCourses(ref List<CustomCourseMapper> list_ccm, int degreeId)
        {
            Degree d = DegreeMapperWebAPI.Degree.Get(degreeId);
            List<Course> list_ucfCourses = DegreeMapperWebAPI.Course.List(d.UCFDegreeId, null);
            if (d.DisplayMultipleSemesters)
            {
                foreach (Course c in list_ucfCourses.Where(x => x.Active && x.Semester > 1))
                {
                    CustomCourseMapper ccm = new CustomCourseMapper(c);
                    list_ccm.Add(ccm);
                }
            } 
            else
            {
                list_ccm = new List<CustomCourseMapper>();
            }
        }
    }
}
