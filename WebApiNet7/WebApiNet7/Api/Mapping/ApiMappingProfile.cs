using AutoMapper;

namespace WebApiNet7.Api.Mapping
{
    public sealed class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<long, string>()
                .ConvertUsing(source => source.ToString());

            CreateMap<string, long>()
                .ConvertUsing(source => long.Parse(source));
        }
    }
}
