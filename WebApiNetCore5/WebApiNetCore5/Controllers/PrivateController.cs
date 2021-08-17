// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//using CustomAuthorizationFailureResponse.Authorization;
using CustomAuthorizationFailureResponse.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiNetCore5.Models;

namespace WebApiNetCore5.Controllers
{
    [ApiController]
    [Route("webapi/[controller]")]
    public class PrivateController : ControllerBase
    {
        [HttpGet("customPolicyWithCustomForbiddenMessage")]
        [Authorize(Policy = SamplePolicyNames.CustomPolicyWithCustomForbiddenMessage)]
        public string GetWithCustomPolicyWithCustomForbiddenMessage()
        {
            return "Hello world from GetWithCustomPolicyWithCustomForbiddenMessage";
        }

        [HttpGet("customPolicy")]
        [Authorize(Policy = SamplePolicyNames.CustomPolicy)]
        public string GetWithCustomPolicy()
        {
            return "Hello world from GetWithCustomPolicy";
        }

        [HttpPost("customLoginPolicy")]
        [Authorize(Policy = SamplePolicyNames.CustomLogin)]
        public IActionResult GetWithCustomLoginPolicy([FromBody] ReqMessage req)
        {
            return Ok("HELLO, I SUCCEED IN AUTHENTICATION!\r\nAUTHENTICATION HEADER MUST BE PRESENT TO SUCCEED!");
        }
    }
}