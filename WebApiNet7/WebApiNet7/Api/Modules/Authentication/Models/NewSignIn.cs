﻿using System.ComponentModel.DataAnnotations;

namespace WebApiNet7.Api.Modules.Authentication.Models
{
    public class NewSignIn
    {
        [Required][EmailAddress] public string Email { get; init; } = string.Empty;
        [Required] public string Password { get; init; } = string.Empty;
    }
}
