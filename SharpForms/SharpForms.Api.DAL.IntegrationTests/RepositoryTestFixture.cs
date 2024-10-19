using SharpForms.Api.DAL.Memory;
using SharpForms.Api.BL.MapperProfiles;
using AutoMapper;
using SharpForms.Api.DAL.Memory.Repositories;

namespace SharpForms.Api.DAL.IntegrationTests;

public class RepositoryTestFixture
{
    protected readonly IMapper _mapper;
    protected readonly Storage _storage;

    protected readonly AnswerRepository _answerRepository;
    protected readonly CompletedFormRepository _completedFormRepository;
    protected readonly FormRepository _formRepository;
    protected readonly QuestionRepository _questionRepository;
    protected readonly UserRepository _userRepository;
    protected readonly SelectOptionRepository _selectOptionRepository;

    public RepositoryTestFixture()
    {
        _storage = new Storage();

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

        _answerRepository = new AnswerRepository(_storage);
        _completedFormRepository = new CompletedFormRepository(_storage, _mapper);
        _formRepository = new FormRepository(_storage, _mapper);
        _questionRepository = new QuestionRepository(_storage, _mapper);
        _userRepository = new UserRepository(_storage, _mapper);
        _selectOptionRepository = new SelectOptionRepository(_storage, _mapper);
    }
}
