using Microsoft.AspNetCore.Components;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.User;
using SharpForms.Web.BL.ApiClients;

namespace SharpForms.Web.App.Pages.User;

public partial class UserEditPage : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public IUserApiClient UserApiClient { get; set; } = null!;

    [Parameter] public Guid Id { get; init; }

    public UserDetailModel Data { get; set; } = GetNewUserModel();

    protected override async Task OnInitializedAsync()
    {
        if (Id != Guid.Empty)
        {
            Data = await UserApiClient.UserGetAsync(Id, "en");
        }

        await base.OnInitializedAsync();
    }

    public async Task Save()
    {
        if (Id != Guid.Empty)
        {
            await UserApiClient.UserPutAsync("en", Data);
            NavigationManager.NavigateTo($"/users/{Id}");
        }
        else
        {
            var createdId = await UserApiClient.UserPostAsync("en", Data);
            NavigationManager.NavigateTo($"/users/{createdId}");
        }
    }

    private static UserDetailModel GetNewUserModel()
        => new() { Id = Guid.NewGuid(), Name = string.Empty, PhotoUrl = string.Empty, Role = UserRole.General };
}
