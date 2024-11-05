using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Models.SelectOption;
using SharpForms.Api.DAL.Common.Entities.Interfaces;

namespace SharpForms.Api.BL.Facades.Question
{
    public interface ISelectOptionFacade : IDetailModelFacade<SelectOptionEntity, SelectOptionModel>
    {
        public IList<SelectOptionModel> GetByQuestionId(Guid questionId);
    }
}
