using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.Question;

namespace SharpForms.Api.BL.MapperProfiles
{
    public class QuestionMapperProfile : Profile
    {
        public QuestionMapperProfile()
        {
            CreateMap<QuestionEntity, QuestionDetailModel>()
                .ForMember(dest => dest.FormName,
                    opt => opt.MapFrom(src => src.Form!.Name))
                .ForMember(dest => dest.Options, opt => opt.Ignore());
            CreateMap<QuestionEntity, QuestionListModel>();
            CreateMap<QuestionDetailModel, QuestionEntity>();
            CreateMap<QuestionListModel, QuestionEntity>();
        }
    }
}
