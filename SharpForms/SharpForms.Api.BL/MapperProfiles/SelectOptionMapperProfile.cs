using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.SelectOption;

namespace SharpForms.Api.BL.MapperProfiles
{
    public class SelectOptionMapperProfile : Profile
    {
        public SelectOptionMapperProfile()
        {
            // Map from SelectOptionEntity to SelectOptionModel
            CreateMap<SelectOptionEntity, SelectOptionModel>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Option));

            CreateMap<SelectOptionModel, SelectOptionEntity>()
                .ForMember(dest => dest.Question, opt => opt.Ignore())
                .ForMember(dest => dest.Option, opt => opt.MapFrom(src => src.Value));

            CreateMap<SelectOptionEntity, SelectOptionEntity>();
        }
    }
}
