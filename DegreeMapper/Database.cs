using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DegreeMapperWebAPI
{
    public class Database
    {
        public static string DC_DegreeMapping { get { return ConfigurationManager.ConnectionStrings["Connection:DC_DegreeMapping"].ConnectionString; } }
    }
}
