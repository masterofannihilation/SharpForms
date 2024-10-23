using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.BL.Facades.Form;
using SharpForms.Api.BL.Facades.User;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Api.DAL.Memory.Repositories;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.User;

namespace SharpForms.Api.BL.Facades.Answer;

public class CompletedFormDetailFacade(
    ICompletedFormRepository completedFormRepository,
    IMapper mapper,
    IUserListFacade userListFacade,
    IAnswerListFacade answerListFacade)
    : DetailModelFacadeBase<CompletedFormEntity, CompletedFormDetailModel>(completedFormRepository, mapper), ICompletedFormDetailFacade
{
    private readonly IMapper _mapper = mapper;

    public override CompletedFormDetailModel? GetById(Guid id)
    {
        var entity = completedFormRepository.GetById(id);
        if (entity == null) return null;
        var model = _mapper.Map<CompletedFormDetailModel>(entity);
        if (model == null) return null;

        if(entity.Form == null) return null;
        model.FormName = entity.Form.Name;

        if(entity.User != null)
            model.User = userListFacade.GetById(entity.User.Id);
        if (entity.Answers != null)
            model.Answers = answerListFacade.GetAll(entity.Id, null);

        return model;
    }
}
