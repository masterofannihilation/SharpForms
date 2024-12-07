using Microsoft.AspNetCore.Components;
using SharpForms.Common.Models.Form;
using SharpForms.Web.BL.ApiClients;

namespace SharpForms.Web.App.Pages.Form;

public partial class FormDetailPage : ComponentBase
{
    [Parameter] public Guid Id { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IFormApiClient FormApiClient { get; set; } = null!;
    private FormDetailModel? Form { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Form = await FormApiClient.FormGetAsync(Id, "en");
        await base.OnInitializedAsync();
    }
    private async Task DeleteAsync()
    {
        await FormApiClient.FormDeleteAsync(Id, "en");
        NavigationManager.NavigateTo("/forms");
    }
}

