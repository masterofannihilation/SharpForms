using System.Globalization;
using Microsoft.AspNetCore.Localization;
using SharpForms.Api.BL;
using SharpForms.Api.DAL.Memory;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Extensions;
using AutoMapper;
using SharpForms.Api.BL.Facades.User;
using SharpForms.Api.BL.Facades.Form;
using SharpForms.Api.BL.Facades.Question;
using SharpForms.Api.BL.Facades.Answer;
using SharpForms.Api.App.Extensions;
using SharpForms.Api.App.Processors;
using SharpForms.Api.BL.Facades.CompletedForm;

var builder = WebApplication.CreateBuilder();

ConfigureCors(builder.Services);
ConfigureLocalization(builder.Services);

ConfigureOpenApiDocuments(builder.Services);
ConfigureDependencies(builder.Services, builder.Configuration);
ConfigureAutoMapper(builder.Services);

var app = builder.Build();

UseDevelopmentSettings(app);
UseSecurityFeatures(app);
UseLocalization(app);
UseRouting(app);
UseEndpoints(app);
UseOpenApi(app);

app.Run();
return;

void ConfigureCors(IServiceCollection serviceCollection)
{
    serviceCollection.AddCors(options =>
    {
        options.AddDefaultPolicy(o =>
            o.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
    });
}

void ConfigureLocalization(IServiceCollection serviceCollection)
{
    serviceCollection.AddLocalization(options => options.ResourcesPath = string.Empty);
}

void ConfigureOpenApiDocuments(IServiceCollection serviceCollection)
{
    serviceCollection.AddEndpointsApiExplorer();
    serviceCollection.AddOpenApiDocument(settings =>
        settings.OperationProcessors.Add(new RequestCultureOperationProcessor()));
}

void ConfigureDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
{
    serviceCollection.AddInstaller<ApiDalMemoryInstaller>();
    serviceCollection.AddInstaller<ApiBlInstaller>();
}

void ConfigureAutoMapper(IServiceCollection serviceCollection)
{
    serviceCollection.AddAutoMapper(typeof(EntityBase), typeof(ApiBlInstaller));
}

void ValidateAutoMapperConfiguration(IServiceProvider serviceProvider)
{
    var mapper = serviceProvider.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();
}

void UseEndpoints(WebApplication application)
{
    var endpointsBase = application.MapGroup("api")
        .WithOpenApi();

    UseUserEndpoints(endpointsBase);
    UseFormEndpoints(endpointsBase);
    UseCompletedFormEndpoints(endpointsBase);
    UseQuestionEndpoints(endpointsBase);
    UseAnswerEndpoints(endpointsBase);
}

void UseUserEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var userEndpoints = routeGroupBuilder.MapGroup("user")
        .WithTags("user");

    userEndpoints.MapGet("",
        (string? name, IUserListFacade facade) => name != null ? facade.SearchAllByName(name) : facade.GetAll());
}

void UseFormEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var formEndpoints = routeGroupBuilder.MapGroup("form")
        .WithTags("form");

    formEndpoints.MapGet("",
        (string? name, IFormListFacade facade) => name != null ? facade.SearchAllByName(name) : facade.GetAll());
}

void UseCompletedFormEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var compFormEndpoints = routeGroupBuilder.MapGroup("completedForm")
        .WithTags("completedForm");

    compFormEndpoints.MapGet("", (string? formId, string? userId, ICompletedFormListFacade facade) =>
    {
        if (formId != null)
        {
            return facade.GetAllCopletionsOfForm(new Guid(formId));
        }

        if (userId != null)
        {
            return facade.GetAllCompletionsMadeByUser(new Guid(userId));
        }

        return facade.GetAll();
    });
}

void UseQuestionEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var formEndpoints = routeGroupBuilder.MapGroup("question")
        .WithTags("question");

    formEndpoints.MapGet("", (string? formId, string? text, string? desc, IQuestionListFacade facade) =>
    {
        Guid? formGuid = formId != null ? new Guid(formId) : null;
        return facade.GetAll(formGuid, text, desc);
    });
}

void UseAnswerEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var formEndpoints = routeGroupBuilder.MapGroup("answer")
        .WithTags("answer");

    formEndpoints.MapGet("", (string? compFormId, string? questionId, IAnswerListFacade facade) =>
    {
        Guid? compFormGuid = compFormId != null ? new Guid(compFormId) : null;
        Guid? questionGuid = questionId != null ? new Guid(questionId) : null;

        return facade.GetAll(compFormGuid, questionGuid);
    });
}

void UseDevelopmentSettings(WebApplication application)
{
    var environment = application.Services.GetRequiredService<IWebHostEnvironment>();

    if (environment.IsDevelopment())
    {
        application.UseDeveloperExceptionPage();
    }
}

void UseSecurityFeatures(IApplicationBuilder application)
{
    application.UseCors();
    application.UseHttpsRedirection();
}

void UseLocalization(IApplicationBuilder application)
{
    application.UseRequestLocalization(new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture(new CultureInfo("en")),
        SupportedCultures = new List<CultureInfo> { new("en"), new("cs") }
    });

    application.UseRequestCulture();
}

void UseRouting(IApplicationBuilder application)
{
    application.UseRouting();
}

void UseOpenApi(IApplicationBuilder application)
{
    application.UseOpenApi();
    application.UseSwaggerUi();
}


// Make the implicit Program class public so test projects can access it
public partial class Program
{
}
