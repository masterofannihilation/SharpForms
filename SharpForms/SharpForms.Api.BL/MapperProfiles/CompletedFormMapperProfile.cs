using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.CompletedForm;

namespace SharpForms.Api.BL.MapperProfiles
{
    public class CompletedFormMapperProfile : Profile
    {
        public CompletedFormMapperProfile()
        {
            CreateMap<CompletedFormEntity, CompletedFormDetailModel>()
                .ForMember(dest => dest.FormName,
                    opt => opt.MapFrom(src => src.Form!.Name));
            CreateMap<CompletedFormEntity, CompletedFormListModel>();
            CreateMap<CompletedFormDetailModel, CompletedFormEntity>();
            CreateMap<CompletedFormListModel, CompletedFormEntity>();
        }
    }
}
