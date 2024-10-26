using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.Answer;
using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.User;

namespace SharpForms.Api.BL.MapperProfiles
{
    public class CompletedFormMapperProfile : Profile
    {
        public CompletedFormMapperProfile()
        {
            // Map from CompletedFormEntity to CompletedFormDetailModel
            CreateMap<CompletedFormEntity, CompletedFormDetailModel>()
                .ForMember(dest => dest.FormName, opt => opt.MapFrom(src => src.Form != null ? src.Form.Name : string.Empty));

            CreateMap<CompletedFormEntity, CompletedFormListModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : null))
                .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => src.CompletedDate));

            CreateMap<CompletedFormDetailModel, CompletedFormEntity>()
                .ForMember(dest => dest.Form, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Answers, opt => opt.Ignore());

            CreateMap<CompletedFormListModel, CompletedFormEntity>()
                .ForMember(dest => dest.Form, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Answers, opt => opt.Ignore());

            CreateMap<CompletedFormEntity, CompletedFormEntity>();
        }
    }
}
