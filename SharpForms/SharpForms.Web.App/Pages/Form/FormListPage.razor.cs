using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Components;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.User;
using SharpForms.Web.BL.ApiClients;

namespace SharpForms.Web.App.Pages.Form;

public partial class FormListPage : ComponentBase
{
    [Inject] private IFormApiClient FormApiClient { get; set; } = null!;
    [Inject] private IUserApiClient UserApiClient { get; set; } = null!;
    private string SearchQuery { get; set; } = string.Empty;
    private ICollection<UserListModel> Users { get; set; } = [];
    private ICollection<FormListModel> Forms { get; set; } = [];
    
    protected override async Task OnInitializedAsync()
    {
        Users = await UserApiClient.UserGetAsync(SearchQuery, "en");

        Forms = await FormApiClient.FormGetAsync(SearchQuery, "en");

        await base.OnInitializedAsync();
    }

    protected async Task SearchForms()
    {
        Forms = await FormApiClient.FormGetAsync(SearchQuery, "en");
    }
}
