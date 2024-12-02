using Microsoft.AspNetCore.Components;
using SharpForms.Common.Models.User;
using SharpForms.Web.BL.ApiClients;

namespace SharpForms.Web.App.Pages.User;

public partial class UserListPage : ComponentBase
{
    [Inject] private IUserApiClient UserApiClient { get; set; } = null!;

    private ICollection<UserListModel> Users { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        Users = await UserApiClient.UserGetAsync("", "en");

        await base.OnInitializedAsync();
    }
}
