using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DegreeMapperWebAPI;

namespace DegreeMapperWebAPI
{
    public class DegreeList
    {
        public int CatalogId { get; set; }
        public string CatalogYear { get; set; }

        public int DegreeId { get; set; }
        public string Degree { get; set; }

        public int InstitutionId { get; set; }
        public string Institution { get; set; }

        public int CollegeId { get; set; }
        public string College { get; set; }

        public int UCFDegreeId { get; set; }

        public DegreeList()
        { 
        
        }

        public static List<DegreeList> List(int? catalogId, int? ucfDegreeId)
        {
                List<DegreeList> list_dl = new List<DegreeList>();
                using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "GetDegreeList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (catalogId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@CatalogId", catalogId.Value);
                    }
                    if (ucfDegreeId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@UCFDegreeId", ucfDegreeId.Value);
                    }
                SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            DegreeList dl = new DegreeList();
                            bool active = true;
                            active = Convert.ToBoolean(dr["Active"].ToString());
                            if (active)
                            {
                                Set(dr, ref dl);
                                list_dl.Add(dl);
                            }
                        }
                    }
                    cn.Close();
                }
                return list_dl;
        }

        private static void Set(SqlDataReader dr, ref DegreeList dl)
        {
            if (dr.HasRows)
            {
                dl.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());
                dl.Degree = dr["Degree"].ToString();
                dl.CatalogId = Convert.ToInt32(dr["CatalogId"].ToString());
                dl.CatalogYear = dr["CatalogYear"].ToString();
                dl.College = dr["College"].ToString();
                dl.CollegeId = (string.IsNullOrEmpty(dr["CollegeId"].ToString())) ? 0 : Convert.ToInt32(dr["CollegeId"].ToString());
                dl.Institution = dr["Institution"].ToString();
                dl.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
                dl.UCFDegreeId = (string.IsNullOrEmpty(dr["UCFDegreeId"].ToString())) ? 0 : Convert.ToInt32(dr["UCFDegreeId"].ToString());
            }
        }
    }
}
