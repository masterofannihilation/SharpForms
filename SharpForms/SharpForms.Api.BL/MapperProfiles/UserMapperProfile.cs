using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.User;

namespace SharpForms.Api.BL.MapperProfiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            // Map from UserEntity to UserDetailModel
            CreateMap<UserEntity, UserDetailModel>();
            CreateMap<UserEntity, UserListModel>();
            CreateMap<UserEntity, UserEntity>();

            CreateMap<UserDetailModel, UserEntity>()
                .ForMember(dest => dest.CompletedForms, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedForms, opt => opt.Ignore());
        }
    }
}
