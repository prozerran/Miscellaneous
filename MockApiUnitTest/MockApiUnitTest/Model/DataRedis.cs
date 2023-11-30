using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MockApiUnitTest.Model
{
    public class WebSite
    {
        public string Url { get; set; } = null!;
    }

    public class DataRedis
    {
        public string Uid { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Age { get; set; } = 0;
        public string Company { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public List<WebSite> Website { get; set; } = null!;
    }

    public class DataDB
    {
        [Key]
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Age { get; set; } = 0;
        public string Company { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public List<WebSite> Website { get; set; } = null!;
    }
}
