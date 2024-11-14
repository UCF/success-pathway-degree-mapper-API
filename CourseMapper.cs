using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

namespace DegreeMapperWebAPI
{
    public class CourseMapper
    {
        public struct DisplayType
        {
            public static string Default { get { return "Default"; } }
            public static string Alternate { get { return "Alternate"; } }
            public static string SelectOne { get { return "Select One"; } }
            public static string SelectTwo { get { return "Select Two"; } }
            public static string OR { get { return "OR"; } }
            public static string AND { get { return "AND"; } }
        }

        public int Id { get; set; }
        public int DegreeId { get; set; }
        public string Degree { get; set; }

        public int CatalogyId { get; set; }
        public string CatalogYear { get; set; }

        public List<int> UCFCourseIds { get; set; }
        public List<Course> UCFCourses { get; set; }

        public List<int> PartnerCourseIds { get; set; }
        public List<Course> PartnerCourses { get; set; }

        public List<int> AlternateUCFCourseIds { get; set; }
        public List<Course> AlternateUCFCourses { get; set; }

        public List<int> AlternatePartnerCourseIds { get; set; }
        public List<Course> AlternatePartnerCourses { get; set; }

        public string DisplayName { get; set; }
        public int DisplayValue { get; set; }

        public string AlternateDisplayName { get; set; }
        public int AlternateDisplayValue { get; set; }

        public List<int> Alternate2PartnerCourseIds { get; set; }
        public List<int> Alternate2UCFCourseIds { get; set; }
        public List<Course> Alternate2PartnerCourses { get; set; }
        public List<Course> Alternate2UCFCourses { get; set; }
        public string Alternate2DisplayName { get; set; }
        public int Alternate2DisplayValue { get; set; }

        public List<int> Alternate3PartnerCourseIds { get; set; }
        public List<int> Alternate3UCFCourseIds { get; set; }
        public List<Course> Alternate3PartnerCourses { get; set; }
        public List<Course> Alternate3UCFCourses { get; set; }
        public string Alternate3DisplayName { get; set; }
        public int Alternate3DisplayValue { get; set; }

        public List<int> Alternate4PartnerCourseIds { get; set; }
        public List<int> Alternate4UCFCourseIds { get; set; }
        public List<Course> Alternate4PartnerCourses { get; set; }
        public List<Course> Alternate4UCFCourses { get; set; }
        public string Alternate4DisplayName { get; set; }
        public int Alternate4DisplayValue { get; set; }


        public List<int> Alternate5PartnerCourseIds { get; set; }
        public List<int> Alternate5UCFCourseIds { get; set; }
        public List<Course> Alternate5PartnerCourses { get; set; }
        public List<Course> Alternate5UCFCourses { get; set; }
        public string Alternate5DisplayName { get; set; }
        public int Alternate5DisplayValue { get; set; }


        public int InstitutionId { get; set; }
        public string Institution { get; set; }

        public int? CloneCourseMapperId { get; set; }

        public CourseMapper()
        {
            PartnerCourseIds = new List<int>();
            PartnerCourses = new List<Course>();

            UCFCourseIds = new List<int>();
            UCFCourses = new List<Course>();

            AlternatePartnerCourseIds = new List<int>();
            AlternatePartnerCourses = new List<Course>();

            AlternateUCFCourseIds = new List<int>();
            AlternateUCFCourses = new List<Course>();

            DisplayValue = 0;
        }

        public CourseMapper(int degreeId)
        {
            Degree d = DegreeMapperWebAPI.Degree.Get(degreeId);
            DegreeId = d.Id;
            Degree = d.Name;
            Institution = d.Institution;
            InstitutionId = d.InstitutionId;
            CatalogYear = d.CatalogYear;
            CatalogyId = d.CatalogId;

            #region primary
            PartnerCourseIds = new List<int>();
            PartnerCourses = new List<Course>();

            UCFCourseIds = new List<int>();
            UCFCourses = new List<Course>();
            #endregion

            #region alternate
            AlternateDisplayValue = 0;
            AlternatePartnerCourseIds = new List<int>();
            AlternatePartnerCourses = new List<Course>();
            AlternateUCFCourseIds = new List<int>();
            AlternateUCFCourses = new List<Course>();
            #endregion

            #region alternate 2
            Alternate2DisplayValue = 0;
            Alternate2PartnerCourseIds = new List<int>();
            Alternate2PartnerCourses = new List<Course>();
            Alternate2UCFCourseIds = new List<int>();
            Alternate2UCFCourses = new List<Course>();
            #endregion

            #region alternate 3
            Alternate3DisplayValue = 0;
            Alternate3PartnerCourseIds = new List<int>();
            Alternate3PartnerCourses = new List<Course>();
            Alternate3UCFCourseIds = new List<int>();
            Alternate3UCFCourses = new List<Course>();
            #endregion

            #region alternate 4
            Alternate4DisplayValue = 0;
            Alternate4PartnerCourseIds = new List<int>();
            Alternate4PartnerCourses = new List<Course>();
            Alternate4UCFCourseIds = new List<int>();
            Alternate4UCFCourses = new List<Course>();
            #endregion

            #region alternate 5
            Alternate5DisplayValue = 0;
            Alternate5PartnerCourseIds = new List<int>();
            Alternate5PartnerCourses = new List<Course>();
            Alternate5UCFCourseIds = new List<int>();
            Alternate5UCFCourses = new List<Course>();
            #endregion

            DisplayValue = 0;
        }

        public static List<CourseMapper> List(int? degreeId, int? id, int? catalogId)
        {
            List<CourseMapper> list_cm = new List<CourseMapper>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetCourseMapper";
                cmd.CommandType = CommandType.StoredProcedure;
                if (degreeId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@DegreeId", degreeId.Value);
                }
                if (id.HasValue)
                {
                    cmd.Parameters.AddWithValue("@Id", id.Value);
                }
                if (catalogId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CatalogId", catalogId.Value);
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        CourseMapper cm = new CourseMapper();
                        SetCourseMapper(dr, ref cm);
                        list_cm.Add(cm);
                    }
                }
                cn.Close();
            }
            return list_cm;
        }

        public static CourseMapper Get(int id)
        {
            CourseMapper cm = List(null, id, null).FirstOrDefault();
            return cm;
        }

        private static void SetCourseMapper(SqlDataReader dr, ref CourseMapper cm)
        {
            if (dr.HasRows)
            {
                cm.Id = Convert.ToInt32(dr["Id"].ToString());
                cm.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());

                #region Primary
                cm.DisplayValue = Convert.ToInt32(dr["DisplayValue"].ToString());
                cm.DisplayName = SetDisplayName(cm.DisplayValue);
                if (!string.IsNullOrEmpty(dr["PartnerCourseIds"].ToString()))
                {
                    cm.PartnerCourseIds = dr["PartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.PartnerCourses = SetCourse(cm.PartnerCourseIds);
                }

                if (!string.IsNullOrEmpty(dr["UCFCourseIds"].ToString()))
                {
                    cm.UCFCourseIds = dr["UCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.UCFCourses = SetCourse(cm.UCFCourseIds);
                }
                #endregion
                #region Alternate
                cm.AlternateDisplayValue = Convert.ToInt32(dr["AlternateDisplayValue"].ToString());
                cm.AlternateDisplayName = SetDisplayName(cm.AlternateDisplayValue);
                if (!string.IsNullOrEmpty(dr["AlternatePartnerCourseIds"].ToString()))
                {
                    cm.AlternatePartnerCourseIds = dr["AlternatePartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.AlternatePartnerCourses = SetCourse(cm.AlternatePartnerCourseIds);
                }

                if (!string.IsNullOrEmpty(dr["AlternateUCFCourseIds"].ToString()))
                {
                    cm.AlternateUCFCourseIds = dr["AlternateUCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.AlternateUCFCourses = SetCourse(cm.AlternateUCFCourseIds);
                }
                #endregion
                #region Alternate 2
                cm.Alternate2DisplayValue = Convert.ToInt32(dr["Alternate2DisplayValue"].ToString());
                cm.Alternate2DisplayName = SetDisplayName(cm.Alternate2DisplayValue);
                if (!string.IsNullOrEmpty(dr["Alternate2PartnerCourseIds"].ToString()))
                {
                    cm.Alternate2PartnerCourseIds = dr["Alternate2PartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate2PartnerCourses = SetCourse(cm.Alternate2PartnerCourseIds);
                }
                if (!string.IsNullOrEmpty(dr["Alternate2UCFCourseIds"].ToString()))
                {
                    cm.Alternate2UCFCourseIds = dr["Alternate2UCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate2UCFCourses = SetCourse(cm.Alternate2UCFCourseIds);
                }
                #endregion
                #region Alternate 3
                cm.Alternate3DisplayValue = Convert.ToInt32(dr["Alternate3DisplayValue"].ToString());
                cm.Alternate3DisplayName = SetDisplayName(cm.Alternate3DisplayValue);
                if (!string.IsNullOrEmpty(dr["Alternate3PartnerCourseIds"].ToString()))
                {
                    cm.Alternate3PartnerCourseIds = dr["Alternate3PartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate3PartnerCourses = SetCourse(cm.Alternate3PartnerCourseIds);
                }

                if (!string.IsNullOrEmpty(dr["Alternate3UCFCourseIds"].ToString()))
                {
                    cm.Alternate3UCFCourseIds = dr["Alternate3UCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate3UCFCourses = SetCourse(cm.Alternate3UCFCourseIds);
                }
                #endregion

                #region Alternate 4
                cm.Alternate4DisplayValue = Convert.ToInt32(dr["Alternate4DisplayValue"].ToString());
                cm.Alternate4DisplayName = SetDisplayName(cm.Alternate4DisplayValue);
                if (!string.IsNullOrEmpty(dr["Alternate4PartnerCourseIds"].ToString()))
                {
                    cm.Alternate4PartnerCourseIds = dr["Alternate4PartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate4PartnerCourses = SetCourse(cm.Alternate4PartnerCourseIds);
                }

                if (!string.IsNullOrEmpty(dr["Alternate4UCFCourseIds"].ToString()))
                {
                    cm.Alternate4UCFCourseIds = dr["Alternate4UCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate4UCFCourses = SetCourse(cm.Alternate4UCFCourseIds);
                }
                #endregion

                #region Alternate 5
                string alternate5DisplayValue = dr["Alternate5DisplayValue"].ToString();
                cm.Alternate5DisplayValue = (!string.IsNullOrEmpty(alternate5DisplayValue)) ? Convert.ToInt32(dr["Alternate5DisplayValue"].ToString()) : 0;
                cm.Alternate5DisplayName = SetDisplayName(cm.Alternate5DisplayValue);
                if (!string.IsNullOrEmpty(dr["Alternate5PartnerCourseIds"].ToString()))
                {
                    cm.Alternate5PartnerCourseIds = dr["Alternate5PartnerCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate5PartnerCourses = SetCourse(cm.Alternate5PartnerCourseIds);
                }

                if (!string.IsNullOrEmpty(dr["Alternate5UCFCourseIds"].ToString()))
                {
                    cm.Alternate5UCFCourseIds = dr["Alternate5UCFCourseIds"].ToString().Split(',').Select(Int32.Parse).ToList();
                    cm.Alternate5UCFCourses = SetCourse(cm.Alternate5UCFCourseIds);
                }
                #endregion
                cm.Degree = dr["Degree"].ToString();
                cm.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());
                cm.Institution = dr["Institution"].ToString();
                cm.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
                cm.CatalogYear = dr["CatalogYear"].ToString();
                cm.CatalogyId = Convert.ToInt32(dr["CatalogId"].ToString());
                int cloneCourseMapperId;
                Int32.TryParse(dr["CloneCourseMapperId"].ToString(), out cloneCourseMapperId);
                cm.CloneCourseMapperId = cloneCourseMapperId;
                //cm.DisplayValue = Convert.ToInt32(dr["DisplayValue"].ToString());
                //SetDisplayName(ref cm);
                //SetCourse(ref cm);
            }
        }

        public static string SetDisplayName(int value)
        {
            switch (value)
            {
                case 5: return CourseMapper.DisplayType.SelectTwo;
                case 4: return CourseMapper.DisplayType.SelectOne;
                case 3: return CourseMapper.DisplayType.OR;
                case 2: return CourseMapper.DisplayType.AND;
                case 1: return CourseMapper.DisplayType.Alternate;
                default: return CourseMapper.DisplayType.Default;
            }
        }

        private static List<Course> SetCourse(List<int> courseIds)
        {
            List<Course> list_c = new List<Course>();
            foreach (int id in courseIds)
            {
                Course c = Course.Get(id);
                list_c.Add(c);
            }
            return list_c;
        }
    }
}
