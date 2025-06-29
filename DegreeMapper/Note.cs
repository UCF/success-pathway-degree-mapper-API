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
    public class Note
    {
        public struct NoteTypeValue
        {
            public static int Note { get { return 1; } }
            public static int ListItem { get { return 2; } }
            public static int ForeignLanguageRequirement { get { return 3; } }
            public static int AdditionalRequirement { get { return 4; } }
        }

        public struct Message
        {
            public static string Get(string institution)
            {
                return (institution == "UCF") ? Note.Message.ForUCF : Note.Message.ForPartner(institution);
            }
            private static string ForUCF { get { return "Global note for all institutions for this degree."; } }
            private static string ForPartner(string institution)
            {
                return "This note will only display for " + institution;
            }
        }

        public int Id { get; set; }
        public int DegreeId { get; set; }
        [DisplayName("Title")]
        public string Name { get; set; }
        [DisplayName("Content")]
        public string Value { get; set; }
        public bool Required { get; set; }
        [DisplayName("Show title on website")]
        public bool ShowName { get; set; }
        [DisplayName("Display Order")]
        public int OrderBy { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string NID { get; set; }
        public bool Active { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public int InstitutionId { get; set; }
        [DisplayName("Foreign Language Requirement")]
        public bool ForeignLanguageRequirement { get; set; }
        [DisplayName("Display Section")]
        public int Section { get; set; }
        [DisplayName("Note Type")]
        public int NoteType { get; set; }
        public int CatalogId { get; set; }
        [DisplayName("Catalog Year")]
        public string CatalogYear { get; set; }
        public int? CloneNoteId { get; set; }

        public Note()
        {
            DegreeId = 0;
            Required = false;
            Active = true;
            //NID = HttpContext.Current.User.Identity.Name;
            Section = 1;
            ShowName = false;
            OrderBy = 1;
            NoteType = Note.NoteTypeValue.Note;
        }

        public Note(int degreeId)
        {
            Degree d = DegreeMapperWebAPI.Degree.Get(degreeId);
            Active = true;
            this.Degree = d.Name;
            DegreeId = d.Id;
            this.Institution = d.Institution;
            InstitutionId = d.InstitutionId;
            Required = true;
            Active = true;
            //NID = HttpContext.Current.User.Identity.Name;
            Section = 1;
            ShowName = false;
            OrderBy = 1;
            NoteType = Note.NoteTypeValue.Note;
            CatalogYear = d.CatalogYear;
            CatalogId = d.CatalogId;
        }

        public static List<Note> List(int? degreeId, int? catalogId)
        {
            List<Note> list_n = new List<Note>();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetNote";
                cmd.CommandType = CommandType.StoredProcedure;
                if (catalogId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@CatalogId", catalogId.Value);
                }
                if (degreeId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@DegreeId", degreeId.Value);
                }
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Note n = new Note();
                        Set(dr, ref n);
                        list_n.Add(n);
                    }
                }
                cn.Close();
            }
            return list_n.OrderBy(x => x.OrderBy).ThenBy(x => x.Name).ToList();
        }

        public static Note Get(int id)
        {
            Note n = new Note();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetNote";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Set(dr, ref n);
                    }
                }
                cn.Close();
            }
            return n;
        }

        public static Note GetClonedNote(int clonedNoteId)
        {
            Note n = new Note();
            using (SqlConnection cn = new SqlConnection(Database.DC_DegreeMapping))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "GetNote";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClonedNoteId", clonedNoteId);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Set(dr, ref n);
                    }
                }
                cn.Close();
            }
            return n;
        }

        private static void Set(SqlDataReader dr, ref Note n)
        {
            if (dr.HasRows)
            {
                n.Id = Convert.ToInt32(dr["Id"].ToString());
                n.Name = dr["Name"].ToString();
                n.Value = dr["Value"].ToString();
                n.Required = Convert.ToBoolean(dr["Required"].ToString());
                n.AddDate = Convert.ToDateTime(dr["AddDate"].ToString());
                n.UpdateDate = Convert.ToDateTime(dr["UpdateDate"].ToString());
                n.NID = dr["NID"].ToString();
                n.Active = Convert.ToBoolean(dr["Active"].ToString());
                n.Degree = dr["Degree"].ToString();
                n.DegreeId = Convert.ToInt32(dr["DegreeId"].ToString());
                n.Institution = dr["Institution"].ToString();
                n.InstitutionId = Convert.ToInt32(dr["InstitutionId"].ToString());
                n.ShowName = Convert.ToBoolean(dr["ShowName"].ToString());
                n.OrderBy = Convert.ToInt32(dr["OrderBy"].ToString());
                n.ForeignLanguageRequirement = Convert.ToBoolean(dr["ForeignLanguageRequirement"].ToString());
                n.Section = Convert.ToInt32(dr["Section"].ToString());
                n.NoteType = Convert.ToInt32(dr["NoteType"].ToString());
                n.CatalogYear = dr["CatalogYear"].ToString();
                n.CatalogId = Convert.ToInt32(dr["CatalogId"].ToString());
                int cloneNoteId;
                Int32.TryParse(dr["CloneNoteId"].ToString(), out cloneNoteId);
                n.CloneNoteId = cloneNoteId;
            }
        }

        public static string GetNoteTypeValue(int noteTypeValue)
        {
            switch (noteTypeValue)
            {
                case 4: return "Additional Requirements";
                case 3: return "Foreign Langauge Requirement";
                case 2: return "List Item";
                default: return "Note";
            }
        }
    }
}
