using AutoMapper;
using WebApiNet7.Api.Modules.Reports.Models;

namespace WebApiNet7.Api.Modules.Reports
{
    public sealed class ReportsMappingProfile : Profile
    {
        public ReportsMappingProfile()
        {
            //Queries
            CreateMap<RecordsModel, Records>();
        }
    }
}
