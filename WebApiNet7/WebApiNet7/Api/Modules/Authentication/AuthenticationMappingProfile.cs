
using AutoMapper;
using WebApiNet7.Api.Modules.Authentication.Models;

namespace WebApiNet7.Api.Modules.Authentication
{
    public sealed class AuthenticationMappingProfile : Profile
    {
        public AuthenticationMappingProfile()
        {
            CreateMap<AuthenticationResult, AuthenticatedUser>();
        }
    }
}
