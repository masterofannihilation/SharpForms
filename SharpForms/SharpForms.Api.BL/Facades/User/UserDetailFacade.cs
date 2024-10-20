using AutoMapper;
using SharpForms.Api.BL.Facades.Common;
using SharpForms.Api.BL.Facades.Form;
using SharpForms.Api.BL.Facades.CompletedForm;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Common.Enums;
using SharpForms.Common.Models.User;
using SharpForms.Api.DAL.Common.Entities.Interfaces;

namespace SharpForms.Api.BL.Facades.Answer;

public class UserDetailFacade(
    IUserRepository userRepository,
    IMapper mapper,
    ICompletedFormListFacade completedFormListFacade,
    IFormListFacade formListFacade)
    : DetailModelFacadeBase<UserEntity, UserDetailModel>(userRepository, mapper), IUserDetailFacade
{
    private readonly IMapper _mapper = mapper;

    
    public override UserDetailModel? GetById(Guid id)
    {
        var entity = userRepository.GetById(id);
        if (entity == null) return null;
        var model = _mapper.Map<UserDetailModel>(entity);
        if (model == null) return null;

        model.CompletedForms = completedFormListFacade.GetAllCompletionsMadeByUser(model.Id);
        model.CreatedForms = formListFacade.GetAllCreatedBy(model.Id);

        return model;
    }

    public override Guid? Update(UserDetailModel model)
    {
        var entity = userRepository.GetById(model.Id);
        if (entity == null) return null;

        entity.Name = model.Name;
        entity.Role = model.Role;
        entity.PhotoUrl = model.PhotoUrl;

        return userRepository.Update(entity);
    }
}
