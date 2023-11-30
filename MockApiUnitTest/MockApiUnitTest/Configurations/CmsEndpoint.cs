using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockApiUnitTest.Configurations
{
    public class CmsEndpoint
    {
        public string CmsEndpointUrl { get; set; } = string.Empty;
        public string CmsFeaturedUrl { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
    }
}
