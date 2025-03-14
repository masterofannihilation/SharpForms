using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.Question;
using SharpForms.Api.DAL.Common.Entities.Interfaces;

namespace SharpForms.Api.BL.Facades.Question
{
    public interface IQuestionDetailFacade : IDetailModelFacade<QuestionEntity, QuestionDetailModel>
    {
    }
}
