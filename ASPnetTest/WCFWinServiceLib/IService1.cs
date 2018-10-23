using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Activation;

namespace WCFWinServiceLib
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        // http://localhost:3761/Service1.svc/StringRequest/Hello/World
        // http://192.168.8.99/WCFWinService/Service1.svc/StringRequest/Hello/World
        [WebInvoke(Method = "GET", UriTemplate = "StringRequest/{p1}/{p2}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string StringRequest(string p1, string p2);

        [OperationContract]
        // http://localhost:3761/Service1.svc/InvokeRestString/theString
        // http://192.168.8.99/WCFWinService/Service1.svc/InvokeRestString/theString
        [WebInvoke(Method = "GET", UriTemplate = "InvokeRestString/{str}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string InvokeRestString(string str);

        [OperationContract]
        // http://localhost:3761/Service1.svc/InvokeGetString?str=theString
        // http://192.168.8.99/WCFWinService/Service1.svc/InvokeGetString?str=theString
        [WebInvoke(Method = "GET", UriTemplate = "InvokeGetString?str={str}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        string InvokeGetString(string str);

        [OperationContract]
        // http://localhost:3761/Service1.svc/InvokePostString
        [WebInvoke(Method = "POST", UriTemplate = "InvokePostString", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        string InvokePostString(string str); 

        [OperationContract]
        // http://localhost:3761/Service1.svc/Mult?x=100&y=5
        // http://192.168.8.99/WCFWinService/Service1.svc/Mult?x=100&y=5
        [WebInvoke(Method = "GET", UriTemplate = "Mult?x={x}&y={y}", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        long Multiply(long x, long y);

        [OperationContract]
        // http://localhost:3761/Service1.svc/login/name
        // http://192.168.8.99/WCFWinService/Service1.svc/login/name
        [WebGet(UriTemplate = "login/{name}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        LoginResponse GetLogin(string name);

        [OperationContract]
        // http://localhost:3761/Service1.svc/login/
        [WebInvoke(Method = "POST", UriTemplate = "postlogin", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        LoginResponse GetLoginPost(string name);
    }

    [DataContract]
    public class LoginResponse
    {
        [DataMember]
        public String username;
        [DataMember]
        public String encoded_username;
        [DataMember]
        public String windowsname;
        [DataMember]
        public String authtoken;
        [DataMember]
        public String ipaddress;
        [DataMember]
        public String sessionid;
        [DataMember]
        public String authtype;
        [DataMember]
        public long lastlogontime;
        [DataMember]
        public int failedlogonattempt;
        [DataMember]
        public List<String> listString;
    }
}
