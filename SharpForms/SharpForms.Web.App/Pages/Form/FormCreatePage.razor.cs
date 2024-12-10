using System.Net.Mime;
using Microsoft.AspNetCore.Components;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.User;
using SharpForms.Common.Models.SelectOption;
using SharpForms.Web.BL.ApiClients;
using Microsoft.AspNetCore.Components.Authorization;

namespace SharpForms.Web.App.Pages.Form
{
    public partial class FormCreatePage : ComponentBase
    {
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
        [Inject] private IFormApiClient FormApiClient { get; set; } = null!;
        [Inject] private IUserApiClient UserApiClient { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        private UserListModel? CurrentUser { get; set; }
        [Parameter] public Guid Id { get; init; }


        private FormDetailModel FormDetailModel { get; set; } = new()
        {
            Id = Guid.Empty,
            Name = string.Empty,
            OpenSince = null,
            OpenUntil = null,
            Creator = null,
            Questions = new List<QuestionListModel>(),  // Empty for now
            Completions = new List<CompletedFormListModel>()  // Empty for now
        };

        // Variable to store error message for displaying warnings
        private string? _errorMessage;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;
            var username = user.Identity?.Name;
            if (!string.IsNullOrEmpty(username))
            {
                var userList = await UserApiClient.UserGetAsync(username, "en");
                CurrentUser = userList.FirstOrDefault();
            }

            await base.OnInitializedAsync();
        }

        private void ValidateFormDates()
        {
            _errorMessage = null;

            if (FormDetailModel.OpenSince.HasValue && FormDetailModel.OpenSince.Value < DateTime.Today)
            {
                _errorMessage = "The 'OpenSince' date must be today or in the future.";
            }

            if (FormDetailModel.OpenUntil.HasValue && FormDetailModel.OpenUntil.Value < FormDetailModel.OpenSince)
            {
                _errorMessage = "The 'OpenUntil' date must be later than the 'OpenSince' date.";
            }
        }

        private async Task SubmitAsync()
        {
            var authState = await AuthenticationStateTask;
            var user = authState.User;

            try
            {
                ValidateFormDates();
                if (string.IsNullOrWhiteSpace(FormDetailModel.Name))
                {
                    _errorMessage = "The 'Form Name' field is required.";
                }

                if (_errorMessage != null)
                {
                    return;
                }

                FormDetailModel.Id = Guid.NewGuid();

                FormDetailModel.Creator = CurrentUser;  

                await FormApiClient.FormPostAsync("en", FormDetailModel);
                NavigationManager.NavigateTo("/forms");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
