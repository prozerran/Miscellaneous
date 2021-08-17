using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiNetCore5.Controllers
{
    [ApiController]
    public class ErrorController : Controller
    {
        [NonAction]
        [Route("/error")]
        public IActionResult Error()
        {
            return BadRequest("OverrideError");
        }
    }
}
