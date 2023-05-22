using Amazon.Lambda.AspNetCoreServer;

namespace ApiOnLambda
{
    public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
    {
        protected override void Init(IWebHostBuilder builder)
        {
            builder.UseStartup<StartupBase>();
        }
    }
}
