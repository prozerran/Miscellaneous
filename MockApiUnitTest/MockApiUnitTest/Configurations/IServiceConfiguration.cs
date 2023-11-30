using Microsoft.VisualBasic;
using System.ComponentModel;

namespace MockApiUnitTest.Configurations
{
    public interface IServiceConfiguration
    {
        AWS AWS { get; }

        CmsEndpoint CmsEndpoint { get; }

        EXP EXP { get; }

        string ServiceHost { get; }
    }
}