using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
namespace WebService
{
   public class BaseDAO
    {
        public OracleConnection connection{get; private set;}

        public BaseDAO()
        {
            connection = new OracleConnection();
            connection.ConnectionString = "User Id=luccah0607;Password=Burdeos1;Data Source=XE";
        }
    }
}
