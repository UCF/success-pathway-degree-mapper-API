using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DegreeMapperWebAPI
{
    public class CustomCourseSemester
    {
        #region Properties
        public int Id { get; set; }
        public int InstitutionId { get; set; }
        public int DegreeId { get; set; }
        public int Semester { get; set; }
        public string Term { get; set; }
        public string Note { get; set; } = string.Empty;
        public int CatalogId { get; set; }
        public string CatalogYear { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }

        public int? CloneId { get; set; }
        #endregion

        public CustomCourseSemester()
        { 
            
        }

        public CustomCourseSemester(int degreeId)
        {
            DegreeMapperWebAPI.Degree d = DegreeMapperWebAPI.Degree.Get(degreeId);
            DegreeId = d.Id;
            Degree = d.Name;
            InstitutionId = d.InstitutionId;
            Institution = d.Institution;
            CatalogId = d.CatalogId;
            CatalogYear = d.CatalogYear;
            Note = string.Empty;
        }

        public static List<CustomCourseSemester> List(int? degreeId, int? catalogId)
        {
            List<CustomCourseSemester> list_ccs = new List<CustomCourseSemester>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetCustomCourseSemester";
                cmd.CommandType = CommandType.StoredProcedure;
                if (degreeId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@DegreeId", degreeId.Value);
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
                        CustomCourseSemester ccs = new CustomCourseSemester();
                        set(dr, ref ccs);
                        list_ccs.Add(ccs);
                    }
                }
                cn.Close();
            }
            return list_ccs;
        }

        public static void set(SqlDataReader dr, ref CustomCourseSemester ccs)
        {
            if (dr.HasRows)
            {
                ccs.Id = Convert.ToInt32(dr["Id"].ToString());
                ccs.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
                ccs.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());
                ccs.Degree = dr["Degree"].ToString();
                ccs.Semester = Convert.ToInt32(dr["Semester"].ToString());
                ccs.Term = dr["Term"].ToString();
                ccs.Note = dr["Note"].ToString();
                ccs.CatalogId = Convert.ToInt32(dr["CatalogId"].ToString());
                ccs.CatalogYear = dr["CatalogYear"].ToString();
                ccs.Institution = dr["Institution"].ToString();
                int cloneId;
                Int32.TryParse(dr["CloneId"].ToString(), out cloneId);
                ccs.CloneId = cloneId;
            }
        }

    }
}
