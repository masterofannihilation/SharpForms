using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.Answer;
using SharpForms.Common.Extensions;

namespace SharpForms.Api.BL.MapperProfiles;

public class AnswerMapperProfile : Profile
{
    public AnswerMapperProfile()
    {
        CreateMap<AnswerEntity, AnswerListModel>().MapMember(dst => dst.Order, src => src.Question!.Order);
    }
}
