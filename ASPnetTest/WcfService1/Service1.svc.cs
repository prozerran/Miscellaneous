using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Channels;
/*
using System.Web.Security;
using System.Web;
using System.Configuration;
using System.Security.Principal;
using System.Security.Permissions;
using System.Net;
*/
namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, AddressFilterMode = AddressFilterMode.Any)]
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public string GetString(string str)
        {
            return str;
        }
    }
}
