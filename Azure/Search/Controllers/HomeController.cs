using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Search.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var searchServiceName = "timsearch2018";
            var apiKey = "lkajsdlfkjwleljrwer";

            var searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
            var indexClient = searchClient.Indexes.GetClient("awindex");

            var sp = new SearchParameters() { SearchMode = SearchMode.All };
            var doc = indexClient.Documents.Search("search_string", sp);

            return Json(doc.Results, JsonRequestBehavior.AllowGet);
        }
    }
}