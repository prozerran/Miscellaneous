using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace TestWEB
{
    public class IdentityContract
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int ProductCode { get; set; } // omitted
    }

    //[DataContract]
    //public class Product
    //{
    //    [DataMember]
    //    public string Name { get; set; }
    //    [DataMember]
    //    public decimal Price { get; set; }
    //    [DataMember]
    //    public int ProductCode { get; set; }  // omitted by default
    //}
}