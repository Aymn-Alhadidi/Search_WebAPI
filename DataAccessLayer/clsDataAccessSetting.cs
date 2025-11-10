using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    static class clsDataAccessSetting
    {
        //public static string ConnectionString = "Server = .; Database = SearchAPI; User ID = sa; Password = sa123456";
          public static string ConnectionString = "Server = localhost; Database = SearchAPI; Integrated Security = True;User Id=sa; Password = sa123456; Encrypt=False; TrustServerCertificate = True;";
    }
}
