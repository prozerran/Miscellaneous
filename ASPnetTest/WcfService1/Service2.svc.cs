using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service2" in code, svc and config file together.

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, AddressFilterMode = AddressFilterMode.Any)]    
    public class Service2 : IService2
    {
        public string GetString(string str)
        {
            return str;
        }
    }
}
