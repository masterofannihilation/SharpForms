using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.Answer;
using SharpForms.Common.Enums;
using SharpForms.Api.DAL.Common.Entities.Interfaces;

namespace SharpForms.Api.BL.MapperProfiles
{
    public class AnswerMapperProfile : Profile
    {
        public AnswerMapperProfile()
        {
            // Map from AnswerEntity to AnswerDetailModel
            CreateMap<AnswerEntity, AnswerDetailModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.CompletedForm!.User != null ? src.CompletedForm!.User : null))
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => GetAnswerValue(src)));

            CreateMap<AnswerEntity, AnswerListModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.CompletedForm!.User != null ? src.CompletedForm.User.Name : string.Empty))
                .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => GetAnswerValue(src)))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Question != null ? src.Question.Order : 0));

            CreateMap<AnswerDetailModel, AnswerEntity>()
                .ForMember(dest => dest.CompletedForm, opt => opt.Ignore())
                .ForMember(dest => dest.Question, opt => opt.Ignore())
                .ForMember(dest => dest.SelectOption, opt => opt.Ignore());

            CreateMap<AnswerListModel, AnswerEntity>()
                .ForMember(dest => dest.CompletedForm, opt => opt.Ignore())
                .ForMember(dest => dest.Question, opt => opt.Ignore())
                .ForMember(dest => dest.SelectOption, opt => opt.Ignore());

            CreateMap<AnswerEntity, AnswerEntity>();
        }
        private string GetAnswerValue(AnswerEntity src)
        {
            if (src.TextAnswer != null)
                return src.TextAnswer;

            if (src.NumberAnswer != null)
                return src.NumberAnswer.Value.ToString();

            if (src.SelectOptionId != null && src.SelectOption != null)
                return src.SelectOption?.Option ?? string.Empty;

            return string.Empty;
        }
    }
}
