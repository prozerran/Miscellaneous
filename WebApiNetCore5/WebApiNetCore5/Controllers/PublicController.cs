using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiNetCore5.Filters;
using WebApiNetCore5.Model;
using WebApiNetCore5.Models;

namespace WebApiNetCore5.Controllers
{
    // swagger
    // https://localhost:44343/swagger/index.html

    //[ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("webapi/[controller]")]
    public class PublicController : ControllerBase
    {
        private readonly ILogger<PublicController> _logger;

        public PublicController(ILogger<PublicController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        // https://localhost:44343/webapi/Home
        public IActionResult Get()
        {
            var str = "RESTful WebApi HTTP/HTTPS Json Service Online!";
            var idx = "https://localhost:44343/swagger/index.html";

            var hp = new HomePage { 
                Date = DateTime.Now, 
                Title = str, 
                IndexPage = idx,
                Version = "1.0" };

            return Ok(hp);
        }

        [HttpPost]
        // https://localhost:44343/webapi/Home
        public IActionResult Post([FromBody] ReqMessage req)
        {
            return Ok(req);
        }

        [HttpGet]
        [Route("{id}/exception")]
        // https://localhost:44343/webapi/Public/1/exception
        public IActionResult TryGetException(int id)
        {
            if (id > 10)
            {
                throw new ArgumentException(
                    $"Sorry Not Found for Id: {id}.", nameof(id));
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("{id}/exception_filter")]
        // https://localhost:44343/webapi/Public/1/exception_filter
        public IActionResult TryGetExceptionWithFilter(int id)
        {
            if (id > 10)
            {
                throw new HttpResponseException($"Sorry Not Found for Id: {id}.");
            }
            return Ok();
        }

        [HttpGet]
        [HttpActionFilter]
        [Route("{id}/check_action")]
        // https://localhost:44343/webapi/Public/1/check_action
        public IActionResult CheckAction(int id)
        {
            return Ok();
        }

        [HttpGet]
        [HttpExceptionFilter]
        [Route("{id}/check_exception")]
        // https://localhost:44343/webapi/Public/1/check_exception
        public IActionResult CheckException(int id)
        {
            if (id > 10)
            {
                throw new HttpResponseException($"Sorry Not Found for Id: {id}.");
            }
            return Ok();
        }

        [HttpGet]
        [HttpAuthroizationFilter]
        [Route("{id}/check_authorization")]
        // https://localhost:44343/webapi/Public/1/check_authorization
        public IActionResult CheckAuthorization(int id)
        {
            return Ok();
        }

        [HttpGet]
        [Route("{id}/ignore_filters")]
        // https://localhost:44343/webapi/Public/1/ignore_filters
        public IActionResult IgnoreFilters(int id)
        {
            return Ok();
        }
    }
}
