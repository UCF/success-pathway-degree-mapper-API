using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;

namespace DegreeMapperWebAPI
{
    public class Degree
    {
        #region Properties
        public int Id { get; set; }
        [DisplayName("Institution Id")]
        public int InstitutionId { get; set; }
        public string Name { get; set; }
        [DisplayName("Degree Type")]
        public string DegreeType { get; set; }
        public string GPA { get; set; }
        [DisplayName("Limited Access")]
        public bool LimitedAccess { get; set; }
        [DisplayName("Restricted Access")]
        public bool RestrictedAccess { get; set; }
        public string Description { get; set; }
        [DisplayName("Catalog Year")]
        public string CatalogYear { get; set; }
        public int CatalogId { get; set; }
        public bool Active { get; set; }
        public string Institution { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string NID { get; set; }
        public int? UCFDegreeId { get; set; }
        [DisplayName("UCF Degree Name")]
        public string UCFDegreeName { get; set; }
        [DisplayName("College Name")]
        public string CollegeName { get; set; }
        public int CollegeId { get; set; }
        [DisplayName("Degree URL")]
        public string DegreeURL { get; set; }
        [DisplayName("Catalog URL")]
        public string CatalogUrl { get; set; }
        public int? CloneDegreeId { get; set; }
        #endregion

        public Degree()
        {

        }

        public Degree(int? catalogId)
        {
            Active = true;
            LimitedAccess = false;
            RestrictedAccess = false;
            //NID = HttpContext.Current.User.Identity.Name;
            GPA = "2.0";
            UCFDegreeId = null;
            CatalogId = (catalogId.HasValue) ? catalogId.Value : new Catalog(true).Id;
            CatalogYear = (catalogId.HasValue) ? Catalog.Get(catalogId.Value).Year : new Catalog(true).Year;
        }

        public Degree(int institutionId, int? catalogId)
        {
            Active = true;
            LimitedAccess = false;
            RestrictedAccess = false;
            InstitutionId = institutionId;
            Institution = DegreeMapperWebAPI.Institution.Get(institutionId).Name;
            //NID = HttpContext.Current.User.Identity.Name;
            GPA = "2.0";
            UCFDegreeId = null;
            CatalogId = (catalogId.HasValue) ? catalogId.Value : new DegreeMapperWebAPI.Catalog(true).Id;
            CatalogYear = (catalogId.HasValue) ? DegreeMapperWebAPI.Catalog.Get(catalogId.Value).Year : new DegreeMapperWebAPI.Catalog(true).Year;
        }

        public static List<Degree> List(int? instiutionId, int? catalogId)
        {
            List<Degree> list_d = new List<Degree>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetDegree";
                cmd.CommandType = CommandType.StoredProcedure;
                if (instiutionId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@InstitutionId", instiutionId.Value);
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
                        Degree d = new Degree(null);
                        Set(dr, ref d);
                        list_d.Add(d);
                    }
                }
                cn.Close();
            }
            return list_d;
        }

        public static Degree Get(int id)
        {
            Degree d = new Degree(null);
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetDegree";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Set(dr, ref d);
                    }
                }
                cn.Close();
            }
            return d;
        }

        /// <summary>
        /// returns partner institutions only
        /// institutionId != 1
        /// </summary>
        /// <param name="id"></param>
        /// <param name="catalogId"></param>
        /// <param name="institutionId"></param>
        /// <returns></returns>
        public static List<Degree> GetPartnerDegrees(int? id, int? catalogId, int? institutionId)
        {
            List<Degree> list_degrees = new List<Degree>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetPartnerDegree";
                cmd.CommandType = CommandType.StoredProcedure;
                if (id.HasValue)
                {
                    cmd.Parameters.AddWithValue("@Id", id.Value);
                }
                if (catalogId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CatalogId", catalogId.Value);
                }
                if (institutionId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@InstitutionId", institutionId.Value);
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Degree d = new Degree();
                        Set(dr, ref d);
                        list_degrees.Add(d);
                    }
                }
                cn.Close();
            }
            return list_degrees;
        }

        private static void Set(SqlDataReader dr, ref Degree d)
        {
            if (dr.HasRows)
            {
                d.Id = Convert.ToInt32(dr["Id"].ToString());
                d.Name = dr["Name"].ToString();
                d.GPA = dr["GPA"].ToString();
                d.DegreeType = dr["DegreeType"].ToString();
                d.LimitedAccess = Convert.ToBoolean(dr["LimitedAccess"].ToString());
                d.RestrictedAccess = Convert.ToBoolean(dr["RestrictedAccess"].ToString());
                d.Description = dr["Description"].ToString();
                d.CatalogYear = dr["CatalogYear"].ToString();
                d.Institution = dr["Institution"].ToString();
                d.Active = Convert.ToBoolean(dr["Active"].ToString());
                d.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
                d.AddDate = Convert.ToDateTime(dr["AddDate"].ToString());
                d.UpdateDate = Convert.ToDateTime(dr["UpdateDate"].ToString());
                d.CollegeName = dr["CollegeName"].ToString();
                d.CollegeId = (!string.IsNullOrEmpty(d.CollegeName)) ? Convert.ToInt32(dr["CollegeId"].ToString()) : 0;
                d.DegreeURL = dr["DegreeUrl"].ToString();
                d.CatalogUrl = dr["CatalogUrl"].ToString();
                d.CatalogId = Convert.ToInt32(dr["CatalogId"].ToString());
                d.NID = dr["NID"].ToString();
                int ucfDegreeId;
                Int32.TryParse(dr["UCFDegreeId"].ToString(), out ucfDegreeId);
                d.UCFDegreeId = ucfDegreeId;
                d.UCFDegreeName = dr["UCFDegreeName"].ToString();
                d.CatalogYear = DegreeMapperWebAPI.Catalog.Get(d.CatalogId).Year;
                int clonedegreeId;
                Int32.TryParse(dr["CloneDegreeId"].ToString(), out clonedegreeId);
                d.CloneDegreeId = clonedegreeId;
            }
        }
    }
}
