using Microsoft.AspNetCore.Components;
using SharpForms.Common.Models.User;
using SharpForms.Web.BL.ApiClients;

namespace SharpForms.Web.App.Pages.User;

public partial class UserDetailPage : ComponentBase
{
    [Parameter] public Guid Id { get; set; }
    [Inject] private IUserApiClient UserApiClient { get; set; } = null!;

    private UserDetailModel? User { get; set; } = null;

    protected override async Task OnInitializedAsync()
    {
        User = await UserApiClient.UserGetAsync(Id, "en");

        await base.OnInitializedAsync();
    }
}
