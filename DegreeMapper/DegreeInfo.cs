using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegreeMapperWebAPI
{
    public class DegreeInfo
    {

        public int Id { get; set; }
        public string Degree { get; set; }

        public int CatalogId { get; set; }
        public string CatalogYear { get; set; }

        public int InstitutionId { get; set; }
        public string Institution { get; set; }

        public int CollegeId { get; set; }
        public string College { get; set; }

        public string CatalogUrl { get; set; }
        public string CollegeUrl { get; set; }
        public string DegreeUrl { get; set; }

        public string GPA { get; set; }
        public bool LimitedAccess { get; set; }
        public bool RestrictedAccess { get; set; }

        public int UCFDegreeId { get; set; }

        public string AdditionalRequirement { get; set; }

        public string ForeignLanguageRequirement { get; set; }

        public List<string> Notes { get; set; }

        public DegreeInfo()
        {
            Notes = new List<string>();
        }

        public static DegreeInfo Get(int id)
        {
            Degree d = DegreeMapperWebAPI.Degree.Get(id);
            DegreeInfo di = new DegreeInfo();
            if (di != null)
            {
                di.Id = d.Id;
                di.Degree = d.Name;
                di.CatalogId = d.CatalogId;
                di.CatalogYear = d.CatalogYear;
                di.College = d.CollegeName;
                di.CatalogUrl = d.CatalogUrl;
                //di.CollegeUrl = ;
                di.DegreeUrl = d.DegreeURL;
                di.Institution = d.Institution;
                di.InstitutionId = d.InstitutionId;
                di.RestrictedAccess = d.RestrictedAccess;
                di.LimitedAccess = d.LimitedAccess;
                if (d.UCFDegreeId.HasValue)
                {
                    di.UCFDegreeId = d.UCFDegreeId.Value;
                }
                di.GPA = d.GPA;
                GetRequirement(ref di);
                GetForeignLanguageRequirement(ref di);
                GetNotes(ref di);
            }
            return di;
        }

        private static void GetRequirement(ref DegreeInfo di)
        {
            int requirementId = DegreeMapperWebAPI.Note.NoteTypeValue.AdditionalRequirement;
            string UCFRequirement = DegreeMapperWebAPI.Note.List(di.UCFDegreeId, null).Where(x => x.NoteType == requirementId).Select(x => x.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(UCFRequirement))
            {
                di.AdditionalRequirement = UCFRequirement;
            }
            /**
              FOR PHASE 1 WE WILL NOT USE PARTNER REQUIREMENTS
              PARTNER REQUIREMENTS IS NOT AVAILABLE
            **/
            //string partnerRequirement = DegreeMapping.Models.Note.List(di.Id).Where(x => x.NoteType == requirementId).Select(x => x.Value).FirstOrDefault();
            //if (!string.IsNullOrEmpty(partnerRequirement))
            //{
            //    di.AdditionalRequirement += string.Format("<div>{0}</div>",partnerRequirement);
            //}
        }

        private static void GetForeignLanguageRequirement(ref DegreeInfo di)
        {
            int requirementId = DegreeMapperWebAPI.Note.NoteTypeValue.ForeignLanguageRequirement;
            string UCFForeingLanguage = DegreeMapperWebAPI.Note.List(di.UCFDegreeId, null).Where(x => x.NoteType == requirementId).Select(x => x.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(UCFForeingLanguage))
            {
                di.ForeignLanguageRequirement = UCFForeingLanguage;
            }
            //string partnerForeignLanguage = DegreeMapping.Models.Note.List(di.Id).Where(x => x.NoteType == requirementId).Select(x => x.Value).FirstOrDefault();
            //if (!string.IsNullOrEmpty(partnerForeignLanguage))
            //{
            //    di.ForeignLanguageRequirement += string.Format("<div>{0}</div>",partnerForeignLanguage);
            //}
        }

        private static void GetNotes(ref DegreeInfo di)
        {
            int listItemdId = DegreeMapperWebAPI.Note.NoteTypeValue.ListItem;
            List<Note> list_ucfNotes = DegreeMapperWebAPI.Note.List(di.UCFDegreeId, null).Where(x => x.NoteType == listItemdId).OrderBy(x => x.OrderBy).ToList();
            if (list_ucfNotes.Count > 0)
            {
                foreach (Note n in list_ucfNotes.OrderBy(x => x.OrderBy))
                {
                    di.Notes.Add(n.Value);
                }

            }
            List<Note> list_partnerNotes = DegreeMapperWebAPI.Note.List(di.Id, null).Where(x => x.NoteType == listItemdId).OrderBy(x => x.OrderBy).ToList();
            if (list_partnerNotes.Count > 0)
            {
                foreach (Note n in list_partnerNotes.OrderBy(x => x.OrderBy))
                {
                    di.Notes.Add(n.Value);
                }
            }
        }
    }
}
