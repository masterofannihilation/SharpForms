using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.BL.Facades;
using SharpForms.Common.Models.Answer;

namespace SharpForms.Api.BL.Facades.Answer
{
    public interface IAnswerSubmitFacade : IAppFacade
    {
        Guid? CreateOrUpdate(AnswerSubmitModel model);
    }
}
