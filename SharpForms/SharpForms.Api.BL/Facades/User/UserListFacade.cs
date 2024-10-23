using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Common.Models.User;

namespace SharpForms.Api.BL.Facades.User;

public class UserListFacade(IUserRepository userRepository, IMapper mapper)
    : ListModelFacadeBase<UserEntity, UserListModel>(userRepository, mapper), IUserListFacade
{
    public List<UserListModel> SearchAllByName(string name)
    {
        return userRepository.GetAllFiltered(name).Select(GetEntityToModel).ToList()!;
    }
}
