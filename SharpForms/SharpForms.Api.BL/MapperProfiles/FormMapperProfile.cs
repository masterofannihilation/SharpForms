using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Extensions;
using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.User;

namespace SharpForms.Api.BL.MapperProfiles
{
    public class FormMapperProfile : Profile
    {
        public FormMapperProfile()
        {
            // Map from FormEntity to FormDetailModel with nested collections and relationships
            CreateMap<FormEntity, FormDetailModel>()
                .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator))
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
                .ForMember(dest => dest.Completions, opt => opt.MapFrom(src => src.Completions));

            CreateMap<FormEntity, FormListModel>()
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator != null ? src.Creator.Name : null))
                .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Questions.Count))
                .ForMember(dest => dest.TimesCompleted, opt => opt.MapFrom(src => src.Completions.Count));

            CreateMap<FormDetailModel, FormEntity>()
                .ForMember(dest => dest.CreatorId, opt => opt.MapFrom(src => src.Creator != null ? (Guid?)src.Creator.Id : null))
                .ForMember(dest => dest.Creator, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore())
                .ForMember(dest => dest.Completions, opt => opt.Ignore());

            CreateMap<FormListModel, FormEntity>()
                .ForMember(dest => dest.Creator, opt => opt.Ignore())
                .ForMember(dest => dest.Questions, opt => opt.Ignore())
                .ForMember(dest => dest.Completions, opt => opt.Ignore());

            CreateMap<FormEntity, FormEntity>();
        }
    }
}
