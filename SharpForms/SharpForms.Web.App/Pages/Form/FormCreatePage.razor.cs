using System.Net.Mime;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.SelectOption;
using SharpForms.Common.Models.User;
using SharpForms.Web.BL.ApiClients;

namespace SharpForms.Web.App.Pages.Form
{
    public partial class FormCreatePage : ComponentBase
    {
        [Inject] private IFormApiClient FormApiClient { get; set; } = null!;
        [Inject] private IUserApiClient UserApiClient { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        
        [Parameter] public Guid Id { get; init; }
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }
        
        private FormDetailModel Form { get; set; } = new()
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
        };
        private bool IsUserAuthenticated { get; set; }
        private UserListModel? CurrentUser { get; set; }
        
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
                    CurrentUser = userList.FirstOrDefault();
                }
            }
            InitForm();
            await base.OnInitializedAsync();
        }

        private void InitForm()
        {
            Form.Id = Guid.Empty;
            Form.Name = string.Empty;
            Form.OpenSince = null;
            Form.OpenUntil = null;
            Form.Creator = CurrentUser;
            Form.Questions = new List<QuestionListModel>();  // Empty for now
            Form.Completions = new List<CompletedFormListModel>(); // Empty for now
        }

        // Variable to store error message for displaying warnings
        private string? _errorMessage;
        
        private void ValidateFormDates()
        {
            _errorMessage = null;

            if (Form.OpenSince.HasValue && Form.OpenSince.Value < DateTime.Today)
            {
                _errorMessage = "The 'OpenSince' date must be today or in the future.";
            }

            if (Form.OpenUntil.HasValue && Form.OpenUntil.Value <= Form.OpenSince)
            {
                _errorMessage = "The 'OpenUntil' date must be later than the 'OpenSince' date.";
            }
        }

        private async Task SubmitAsync()
        {
            try
            {
                ValidateFormDates();
                if (string.IsNullOrWhiteSpace(Form.Name))
                {
                    _errorMessage = "The 'Form Name' field is required.";
                }

                if (_errorMessage != null)
                {
                    return;
                }

                Form.Id = Guid.NewGuid();
                await FormApiClient.FormPostAsync("en", Form);
                NavigationManager.NavigateTo("/forms");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
