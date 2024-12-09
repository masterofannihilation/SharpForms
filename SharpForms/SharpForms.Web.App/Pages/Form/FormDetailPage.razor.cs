using Microsoft.AspNetCore.Components;
using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.Form;
using SharpForms.Web.BL.ApiClients;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using SharpForms.Common.Models.User;

namespace SharpForms.Web.App.Pages.Form;

public partial class FormDetailPage : ComponentBase
{
    [Parameter] public Guid Id { get; set; }
    
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IFormApiClient FormApiClient { get; set; } = null!;
    [Inject] private IUserApiClient UserApiClient { get; set; } = null!;
    [Inject] private ICompletedFormApiClient CompletedFormApiClient { get; set; } = null!;
    private FormDetailModel Form { get; set; }
    private ICollection<CompletedFormListModel> CompletedForms { get; set; } = [];
    
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
        
        
        Form = await FormApiClient.FormGetAsync(Id, "en");
        CompletedForms = await CompletedFormApiClient.CompletedFormGetAsync(Id.ToString(), null, "en");
        await base.OnInitializedAsync();
    }

    private async Task DeleteAsync()
    {
        await FormApiClient.FormDeleteAsync(Id, "en");
        NavigationManager.NavigateTo("/forms");
    }
}
