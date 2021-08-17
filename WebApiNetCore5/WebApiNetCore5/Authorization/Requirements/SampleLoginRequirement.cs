// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authorization;

namespace CustomAuthorizationFailureResponse.Authorization.Requirements
{
    public class SampleLoginRequirement : IAuthorizationRequirement
    {
        public bool IsLoginOk(string authen) 
        {
            return !string.IsNullOrEmpty(authen);
        }
    }
}
