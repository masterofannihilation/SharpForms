using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Extensions;
using SharpForms.Common.Models.Form;

namespace SharpForms.Api.BL.MapperProfiles
{
    public class FormMapperProfile : Profile
    {
        public FormMapperProfile()
        {
            CreateMap<FormEntity, FormDetailModel>()
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator));
            CreateMap<FormEntity, FormListModel>();
            CreateMap<FormDetailModel, FormEntity>();
            CreateMap<FormListModel, FormEntity>();
        }
    }
}
