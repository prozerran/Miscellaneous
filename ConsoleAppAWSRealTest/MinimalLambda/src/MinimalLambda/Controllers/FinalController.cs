using Microsoft.AspNetCore.Mvc;

namespace MinimalLambda.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinalController : Controller
    {
        private readonly ILogger<CalculatorController> _logger;

        public FinalController(ILogger<CalculatorController> logger)
        {
            _logger = logger;
        }

        [HttpGet("getstring/{user}/{pass}")]
        public IActionResult GetString(string user, string pass)
        {
            var str = string.Format($"The Name {user}, with pass {pass}!");
            return Ok(str);
        }
    }
}
