using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.User;

namespace SharpForms.Api.BL.MapperProfiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserEntity, UserDetailModel>();
            CreateMap<UserEntity, UserListModel>();
            CreateMap<UserDetailModel, UserEntity>();
            CreateMap<UserListModel, UserEntity>();
        }
    }
}
