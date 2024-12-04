using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharpForms.Common.Models.User;
using SharpForms.Web.BL.ApiClients;

namespace SharpForms.Web.App.Pages.User;

public partial class UserListPage : ComponentBase
{
    [CascadingParameter] 
    public Task<AuthenticationState> AuthenticationStateTask { get; set; }
    [Inject] private IUserApiClient UserApiClient { get; set; } = null!;

    private string SearchQuery { get; set; } = string.Empty;
    private ICollection<UserListModel> Users { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        var user = authState.User;
        user.FindFirst("role");
        user.IsInRole("admin");
        Users = await UserApiClient.UserGetAsync(SearchQuery, "en");

        await base.OnInitializedAsync();
    }

    private async Task SearchUsers()
    {
        Users = await UserApiClient.UserGetAsync(SearchQuery, "en");
    }
}
