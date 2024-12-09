using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.User;
using SharpForms.Web.BL.ApiClients;

namespace SharpForms.Web.App.Pages.Form;

public partial class FormListPage : ComponentBase
{
    [Inject] private IFormApiClient FormApiClient { get; set; } = null!;
    [Inject] private IUserApiClient UserApiClient { get; set; } = null!;
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
    private string SearchQuery { get; set; } = string.Empty;
    private ICollection<FormListModel> Forms { get; set; } = new List<FormListModel>();
    private UserDetailModel? CurrentUser { get; set; }
    private bool IsUserAuthenticated { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        var user = authState.User;
        IsUserAuthenticated = user.Identity?.IsAuthenticated ?? false;

        if (IsUserAuthenticated)
        {
            var username = user.Identity?.Name;
            if (!string.IsNullOrEmpty(username))
            {
                var userList = await UserApiClient.UserGetAsync(username, "en");
                var userDetail = userList.FirstOrDefault();
                if (userDetail != null)
                {
                    CurrentUser = await UserApiClient.UserGetAsync(userDetail.Id, "en");
                }
            }
        }

        Forms = await FormApiClient.FormGetAsync(SearchQuery, "en");
        await base.OnInitializedAsync();
    }

    protected async Task SearchForms()
    {
        Forms = await FormApiClient.FormGetAsync(SearchQuery, "en");
    }
}
