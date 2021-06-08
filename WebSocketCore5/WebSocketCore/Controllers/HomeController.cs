using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Serilog;
using WebSocketCore.Common;

// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio

namespace WebSocketCore.Controllers
{
    public class HomePage
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
    }

    public class ReqMessage
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    // swagger
    // https://localhost:44327/swagger/index.html

    [ApiController]
    [Route("webapi/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var str = "RESTful WebApi WebSocket HTTP/HTTPS Json Service Online!";
            var hp = new HomePage { Date = DateTime.Now, Title = str, Version = "1.01" };
            Log.Information(str);

            return Ok(hp);
        }

        [HttpPost]
        [Route("GetJsonString")]
        public IActionResult GetJsonString([FromBody] ReqMessage req)
        {
            try
            {
                if (req != null)
                {
                    Log.Information(req.ToJsonString());
                    return Ok(req);
                }
                else
                    return BadRequest("BadRequest Error Message");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.StackTrace);
            }
        }

        [NonAction]
        [Route("SynchronousCall")]
        public IActionResult SynchronousCall()
        {
            return Ok("this is normal call");
        }

        [NonAction]
        [Route("AsynchronousCall")]
        // https://stackoverflow.com/questions/41953102/using-async-await-or-task-in-web-api-controller-net-core
        public async Task<IActionResult> AsynchronousCall()
        {
            var result = await Task.Run(() => "this is async call");
            return Ok(result);
        }
    }
}
