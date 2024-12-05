using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharpForms.Common.Models.User;
using SharpForms.Web.BL.ApiClients;

namespace SharpForms.Web.App.Pages.User;

public partial class UserDetailPage : ComponentBase
{
    [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; }
    [Parameter] public Guid Id { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IUserApiClient UserApiClient { get; set; } = null!;

    private UserDetailModel? User { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        var role = authState.User.FindFirst("role")?.Value;
        if (role != "admin")
        {
            NavigationManager.NavigateTo("/unauthorized");
        }

        User = await UserApiClient.UserGetAsync(Id, "en");

        await base.OnInitializedAsync();
    }

    private async Task DeleteAsync()
    {
        await UserApiClient.UserDeleteAsync(Id, "en");
        NavigationManager.NavigateTo("/users");
    }
}
