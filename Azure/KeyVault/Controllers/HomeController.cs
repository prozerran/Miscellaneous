using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KeyVault.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var astp = new AzureServiceTokenProvider();

            var key = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(astp.KeyVaultTokenCallback));

            var secret = await key.GetSecretAsync("http://......").ConfigureAwait(false);

            ViewBag.Secret = $"Secret: {secret.Value}";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}