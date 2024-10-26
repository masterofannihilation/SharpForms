using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.Answer;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.SelectOption;

namespace SharpForms.Api.BL.MapperProfiles
{
    public class QuestionMapperProfile : Profile
    {
        public QuestionMapperProfile()
        {
            // Mapping from QuestionEntity to QuestionDetailModel
            CreateMap<QuestionEntity, QuestionDetailModel>()
                .ForMember(dest => dest.FormName, opt => opt.MapFrom(src => src.Form != null ? src.Form.Name : string.Empty));

            CreateMap<QuestionDetailModel, QuestionEntity>()
                .ForMember(dest => dest.Form, opt => opt.Ignore())
                .ForMember(dest => dest.Options, opt => opt.Ignore())
                .ForMember(dest => dest.Answers, opt => opt.Ignore());

            CreateMap<QuestionListModel, QuestionEntity>()
                .ForMember(dest => dest.Form, opt => opt.Ignore())
                .ForMember(dest => dest.Options, opt => opt.Ignore())
                .ForMember(dest => dest.Answers, opt => opt.Ignore());

            CreateMap<QuestionEntity, QuestionListModel>();
            CreateMap<QuestionEntity, QuestionEntity>();
        }
    }
}
