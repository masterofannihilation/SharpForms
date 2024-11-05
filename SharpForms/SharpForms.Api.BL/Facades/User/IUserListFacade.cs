using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.User;

namespace SharpForms.Api.BL.Facades.User;

public interface IUserListFacade : IListModelFacade<UserEntity, UserListModel>
{
    List<UserListModel> SearchAllByName(string name);
}
