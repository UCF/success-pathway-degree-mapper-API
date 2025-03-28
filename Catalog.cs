using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DegreeMapperWebAPI
{
    public class Catalog
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public bool Active { get; set; }
        public bool Current { get; set; }
        public bool DisplayOnWeb { get; set; }
        public string UndergraduateCatalogURL { get; set; }

        public Catalog()
        {

        }

        public Catalog(bool current)
        {
            Catalog c = Catalog.List().Where(x => x.Current).FirstOrDefault();
            Id = c.Id;
            Year = c.Year;
            Active = c.Active;
            Current = c.Current;
            DisplayOnWeb = c.DisplayOnWeb;
            UndergraduateCatalogURL = c.UndergraduateCatalogURL;
        }

        public static Catalog Get(int id)
        {
            Catalog c = new Catalog();
            c = List().Where(x => x.Id == id).FirstOrDefault();
            return c;
        }

        public static Catalog Get(string year)
        {
            Catalog c = new Catalog();
            c = List().Where(x => x.Year == year).FirstOrDefault();
            return c;
        }

        public static List<Catalog> List()
        {
            List<Catalog> list_c = new List<Catalog>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetCatalog";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Catalog c = new Catalog();
                        Set(dr, ref c);
                        list_c.Add(c);
                    }
                }
                cn.Close();
            }
            return list_c;
        }

        private static void Set(SqlDataReader dr, ref Catalog c)
        {
            if (dr.HasRows)
            {
                c.Id = Convert.ToInt32(dr["Id"].ToString());
                c.Year = dr["Year"].ToString();
                c.Active = Convert.ToBoolean(dr["Active"].ToString());
                c.Current = Convert.ToBoolean(dr["Current"].ToString());
                c.DisplayOnWeb = Convert.ToBoolean(dr["DisplayOnWeb"].ToString());
                c.UndergraduateCatalogURL = dr["UndergraduateCatalogURL"].ToString();
            }
        }

    }
}
