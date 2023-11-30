using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockApiUnitTest.Configurations
{
    public class ServiceConfiguration : IServiceConfiguration
    {
        public AWS AWS {get; set;}

        public CmsEndpoint CmsEndpoint { get; set; }

        public EXP EXP { get; set; }

        public string ServiceHost { get; set; }
    }
}
