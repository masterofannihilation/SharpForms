using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.Answer;

namespace SharpForms.Api.BL.MapperProfiles
{
    public class AnswerMapperProfile : Profile
    {
        public AnswerMapperProfile()
        {
            CreateMap<AnswerEntity, AnswerDetailModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.CompletedForm!.User));
            CreateMap<AnswerEntity, AnswerListModel>();
            CreateMap<AnswerDetailModel, AnswerEntity>();
            CreateMap<AnswerListModel, AnswerEntity>();
            CreateMap<AnswerSubmitModel, AnswerEntity>();
            CreateMap<AnswerEntity, AnswerSubmitModel>();
        }
    }
}
