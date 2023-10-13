using mRemoteNG.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

/*
 * Command dto for all neeed information to connect to database
 */
namespace mRemoteNG.Model.Config
{
    public class DBConnectionSettingsDTO
    {
        public string sqlType { get; set; }
        public string sqlHost { get; set; }
        public string sqlCatalog { get; set; }
        public string sqlUsername { get; set; }
        public string sqlPasswordCrypted { get; set; }
        public SecureString encryptionKey { get; set; }
    }
}
