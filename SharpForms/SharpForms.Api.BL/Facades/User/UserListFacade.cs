using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Memory.Repositories;
using SharpForms.Common.Models.User;

namespace SharpForms.Api.BL.Facades.User;

public class UserListFacade(UserRepository userRepository, IMapper mapper)
    : ListModelFacadeBase<UserEntity, UserListModel>(userRepository, mapper), IUserListFacade
{
}
