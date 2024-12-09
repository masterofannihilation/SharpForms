using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using SharpForms.Common.Models.User;
using SharpForms.Web.BL.ApiClients;

namespace SharpForms.Web.App.Pages.User;

public partial class UserListPage : ComponentBase
{
    [Inject] private IUserApiClient UserApiClient { get; set; } = null!;

    private string SearchQuery { get; set; } = string.Empty;
    private ICollection<UserListModel> Users { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        Users = await UserApiClient.UserGetAsync(SearchQuery, "en");
        await base.OnInitializedAsync();
    }

    private async Task SearchUsers()
    {
        Users = await UserApiClient.UserGetAsync(SearchQuery, "en");
    }

    private async Task DeleteUser(Guid userId)
    {
        await UserApiClient.UserDeleteAsync(userId, "en");
        Users = await UserApiClient.UserGetAsync(SearchQuery, "en"); // Refresh list
    }
}
