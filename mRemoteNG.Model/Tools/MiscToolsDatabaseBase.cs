using MySql.Data.Types;
using System.Data.SqlTypes;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public static class MiscToolsDatabase
    {

        public static string DBDate(DateTime Dt, string type)
        {
//            switch (Properties.OptionsDBsPage.Default.SQLServerType)
            switch(type)
            {
                case "mysql":
                    return Dt.ToString("yyyy/MM/dd HH:mm:ss");
                case "mssql":
                default:
                    return Dt.ToString("yyyyMMdd HH:mm:ss");
            }
        }

        public static object DBTimeStampNow(string type)
        {
            //            switch (Properties.OptionsDBsPage.Default.SQLServerType)
            switch (type)
            {
                case "mysql":
                    return new MySqlDateTime(DateTime.Now.ToUniversalTime());
                case "mssql":
                default:
                    return DateTime.Now.ToUniversalTime();
            }
        }

        public static Type DBTimeStampType(string type)
        {
//            switch (Properties.OptionsDBsPage.Default.SQLServerType)
            switch(type)
            {
                case "mysql":
                    return typeof(MySqlDateTime);
                case "mssql":
                default:
                    return typeof(SqlDateTime);
            }
        }
    }
}