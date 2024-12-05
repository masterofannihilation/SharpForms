using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace SharpForms.Web.App.Components;

public partial class LoginDisplay : ComponentBase
{
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private string DisplayName { get; set; } = "Hello logged in user!";

    protected override async Task OnInitializedAsync()
    {
        if (AuthenticationStateProvider is { } authStateProvider)
        {
            authStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
        }

        await UpdateDisplayNameAsync();
    }

    private async Task UpdateDisplayNameAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity?.IsAuthenticated == true)
        {
            DisplayName = "Logged in as " + authState.User.Identity.Name;
        }

        StateHasChanged();
    }

    private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        await UpdateDisplayNameAsync();
    }

    private void BeginLogOut()
    {
        NavigationManager.NavigateToLogout("authentication/logout");
    }

    public void Dispose()
    {
        if (AuthenticationStateProvider is { } authStateProvider)
        {
            authStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }
    }
}
