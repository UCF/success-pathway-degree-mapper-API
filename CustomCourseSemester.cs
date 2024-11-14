using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DegreeMapperWebAPI
{
    public class CustomCourseSemester
    {
        //public int Id { get; set; }
        //public int InstitutionId { get; set; }
        //public int DegreeId { get; set; }
        public int Semester { get; set; }
        public string Term { get; set; }
        //public string SemesterTerm { get; set; }
        public string Note { get; set; }
        //public int CatalogId { get; set; }
        //public string CatalogYear { get; set; }
        //public string Degree { get; set; }
        //public string Institution { get; set; }

        //public int? CloneId { get; set; }


        public CustomCourseSemester()
        {
            this.Note = string.Empty;
        }

        public CustomCourseSemester(int degreeId)
        {
            //Degree d = DegreeMapperWebAPI.Degree.Get(degreeId);
            //this.DegreeId = d.Id;
            //this.Degree = d.Name;
            //this.InstitutionId = d.InstitutionId;
            //this.Institution = d.Institution;
            //this.CatalogId = d.CatalogId;
            //this.CatalogYear = d.CatalogYear;
            this.Note = string.Empty;
        }

        public static CustomCourseSemester Get(int id)
        {
            CustomCourseSemester ccs = new CustomCourseSemester();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetCustomCourseSemester";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        set(dr, ref ccs);
                    }
                }
                cn.Close();
            }
            return ccs;
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
                //ccs.Id = Convert.ToInt32(dr["Id"].ToString());
                //ccs.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
                //ccs.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());
                ccs.Semester = Convert.ToInt32(dr["Semester"].ToString());
                //ccs.SemesterTerm = string.Format("{0}{1}", dr["Term"].ToString(), dr["Semester"].ToString()).Trim(); //dr["Term"].ToString();
                ccs.Term = dr["Term"].ToString();
                ccs.Note = dr["Note"].ToString();
                //ccs.CatalogId = Convert.ToInt32(dr["CatalogId"].ToString());
                //ccs.CatalogYear = dr["CatalogYear"].ToString();
                //ccs.Institution = dr["Institution"].ToString();
                //int cloneId;
                //Int32.TryParse(dr["CloneId"].ToString(), out cloneId);
                //ccs.CloneId = cloneId;
            }
        }
    }
}