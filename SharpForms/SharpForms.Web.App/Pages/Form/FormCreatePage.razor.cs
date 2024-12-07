using System.Net.Mime;
using Microsoft.AspNetCore.Components;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.SelectOption;
using SharpForms.Web.BL.ApiClients;

namespace SharpForms.Web.App.Pages.Form
{
    public partial class FormCreatePage : ComponentBase
    {
        [Inject] private IFormApiClient FormApiClient { get; set; } = null!;
        [Inject] private IUserApiClient UserApiClient { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        
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
        
        private void ValidateFormDates()
        {
            _errorMessage = null;

            if (FormDetailModel.OpenSince.HasValue && FormDetailModel.OpenSince.Value < DateTime.Today)
            {
                _errorMessage = "The 'OpenSince' date must be today or in the future.";
            }

            if (FormDetailModel.OpenUntil.HasValue && FormDetailModel.OpenUntil.Value <= FormDetailModel.OpenSince)
            {
                _errorMessage = "The 'OpenUntil' date must be later than the 'OpenSince' date.";
            }
        }

        private async Task SubmitAsync()
        {
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
