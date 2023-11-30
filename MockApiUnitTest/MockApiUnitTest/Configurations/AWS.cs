using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockApiUnitTest.Configurations
{
    public class AWS
    {
        public SecretManager SecretManager { get; set; }
        public APIKey APIKey { get; set; }
    }
}
