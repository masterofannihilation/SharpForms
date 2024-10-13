using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.SelectOption;

namespace SharpForms.Api.BL.MapperProfiles
{
    public class SelectOptionMapperProfile : Profile
    {
        public SelectOptionMapperProfile()
        {
            CreateMap<SelectOptionEntity, SelectOptionEntity>()
            .ForMember(dest => dest.Question, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<SelectOptionModel, SelectOptionEntity>();
        }
    }
}
