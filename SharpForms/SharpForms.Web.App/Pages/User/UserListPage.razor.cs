using Microsoft.AspNetCore.Components;
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

    public async Task SearchUsers()
    {
        Users = await UserApiClient.UserGetAsync(SearchQuery, "en");
    }
}
