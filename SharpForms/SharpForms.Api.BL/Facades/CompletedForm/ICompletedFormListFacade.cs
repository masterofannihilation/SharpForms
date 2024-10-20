using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.CompletedForm;

namespace SharpForms.Api.BL.Facades.CompletedForm;

public interface ICompletedFormListFacade : IListModelFacade<CompletedFormEntity, CompletedFormListModel>
{
    List<CompletedFormListModel> GetAllCopletionsOfForm(Guid formId);
    List<CompletedFormListModel> GetAllCompletionsMadeByUser(Guid userId);
}
