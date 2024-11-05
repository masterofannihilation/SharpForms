using AutoMapper;
using SharpForms.Api.DAL.Memory.Repositories;
using SharpForms.Api.DAL.Memory;
using SharpForms.Api.DAL.Common.Repositories;
using SharpForms.Api.BL.MapperProfiles;

namespace SharpForms.Api.BL.IntegrationTests.Fixtures;

public class FacadeTestFixture
{
    protected readonly IMapper _mapper;

    protected readonly IAnswerRepository _answerRepository;
    protected readonly ICompletedFormRepository _completedFormRepository;
    protected readonly IFormRepository _formRepository;
    protected readonly IQuestionRepository _questionRepository;
    protected readonly IUserRepository _userRepository;
    protected readonly ISelectOptionRepository _selectOptionRepository;

    public FacadeTestFixture()
    {
        var storage = new Storage();

        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AnswerMapperProfile>();
            cfg.AddProfile<FormMapperProfile>();
            cfg.AddProfile<CompletedFormMapperProfile>();
            cfg.AddProfile<QuestionMapperProfile>();
            cfg.AddProfile<UserMapperProfile>();
            cfg.AddProfile<SelectOptionMapperProfile>();
        });
        _mapper = new Mapper(mapperConfiguration);

        _answerRepository = new AnswerRepository(storage, _mapper);
        _completedFormRepository = new CompletedFormRepository(storage, _mapper);
        _formRepository = new FormRepository(storage, _mapper);
        _questionRepository = new QuestionRepository(storage, _mapper);
        _userRepository = new UserRepository(storage, _mapper);
        _selectOptionRepository = new SelectOptionRepository(storage, _mapper);
    }
}
