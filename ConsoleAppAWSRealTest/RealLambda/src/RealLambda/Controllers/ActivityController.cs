using Microsoft.AspNetCore.Mvc;

//  dotnet lambda deploy-function

// debug
// https://www.youtube.com/watch?v=unt2-B0QQFQ
// https://www.youtube.com/watch?v=MgukevJCzMA

namespace RealLambda.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActivityController : Controller
    {
        private readonly ILogger<ActivityController> _logger;

        public ActivityController(ILogger<ActivityController> logger)
        {
            _logger = logger;
        }

        // https://972oxcdpj9.execute-api.ap-southeast-1.amazonaws.com/Live/activity/test/5/9
        [HttpGet("test/{x}/{y}")]
        public IActionResult Test(int x, int y)
        {
            _logger.LogInformation($"{x} test {y} is {x + y}");

            var str = string.Format($"Lets test {x} and {y}");
            return Ok(str);
        }
    }
}
