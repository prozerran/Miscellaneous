// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using CustomAuthorizationFailureResponse.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using WebApiNetCore5.Models;

namespace CustomAuthorizationFailureResponse.Authorization.Handlers
{
    public class SampleLoginRequirementHandler : AuthorizationHandler<SampleLoginRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SampleLoginRequirement requirement)
        {
            var http = context.Resource as HttpContext;
            var head = http.Request.Headers;

            // Can only read from HEADER or other data, NOT from HTTP body as that will render the actual method to lose it's "stream"
            var auth = head["Authentication"];
          
            var ret = requirement.IsLoginOk(auth);

            if (ret == true)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
