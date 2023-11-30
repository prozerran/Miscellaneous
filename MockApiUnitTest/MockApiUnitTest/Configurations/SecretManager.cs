using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MockApiUnitTest.Configurations
{
    public class SecretManager
    {
        public virtual string ServiceURL { get; set; } = string.Empty;
        public virtual string KeyID { get; set; } = string.Empty;
        public virtual string SecretKey { get; set; } = string.Empty;
        public virtual string Username { get; set; } = string.Empty;
        public virtual string Password { get; set; } = string.Empty;
        public virtual string IvID { get; set; } = string.Empty;
        public virtual string AWSAPIKeyValue { get; set; } = string.Empty;
        public virtual string AWSAccessToken { get; set; } = string.Empty;
    }
}
