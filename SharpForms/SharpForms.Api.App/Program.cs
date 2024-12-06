using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Localization;
using SharpForms.Api.BL;
using SharpForms.Api.DAL.Memory;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SharpForms.Api.BL.Facades.User;
using SharpForms.Api.BL.Facades.Form;
using SharpForms.Api.BL.Facades.CompletedForm;
using SharpForms.Api.BL.Facades.Question;
using SharpForms.Api.BL.Facades.Answer;
using SharpForms.Common.Models.User;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.CompletedForm;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.Answer;
using SharpForms.Api.App.Extensions;
using SharpForms.Api.App.Processors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Localization;
using SharpForms.Common.Enums;

var builder = WebApplication.CreateBuilder();

ConfigureCors(builder.Services);
ConfigureLocalization(builder.Services);

var identityUrl = builder.Configuration.GetSection("IdentityServer")["Url"];
ConfigureAuthentication(builder.Services, identityUrl!);

ConfigureOpenApiDocuments(builder.Services);
ConfigureDependencies(builder.Services, builder.Configuration);
ConfigureAutoMapper(builder.Services);


var app = builder.Build();

UseDevelopmentSettings(app);
UseSecurityFeatures(app);
UseLocalization(app);
UseRouting(app);
UseAuthorization(app);
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

void ConfigureAuthentication(IServiceCollection services, string identityServerUrl)
{
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.Authority = identityServerUrl;
        options.TokenValidationParameters.ValidateAudience = false;
    });

    services.AddAuthorization(options =>
    {
        options.AddPolicy(UserRole.Admin.ToString(), policy =>
        {
            policy.RequireClaim("role", UserRole.Admin.ToString());
        });
    });
    services.AddHttpContextAccessor();
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

    // List users, search by name
    userEndpoints.MapGet("",
        (string? name, IUserListFacade facade, ClaimsPrincipal userClaims) =>
        {
            if (userClaims.Identity != null)
            {
                var userName = userClaims.Identity.Name;
                var role = userClaims.FindFirst(ClaimTypes.Role)?.Value;
                Console.WriteLine($"User Name: {userName}, Role: {role}");
            }
            if (userClaims.HasClaim(c=> c.Type == ClaimTypes.Role))
            {
                
            }
            
            return name != null ? facade.SearchAllByName(name) : facade.GetAll();
        }).RequireAuthorization();

    // Get user detail
    userEndpoints.MapGet("{id:guid}",
        Results<Ok<UserDetailModel>, NotFound<string>> (Guid id, IUserDetailFacade userFacade)
            => userFacade.GetById(id) is { } user
                ? TypedResults.Ok(user)
                : TypedResults.NotFound($"User with ID {id} not found."));

    // Create new user
    userEndpoints.MapPost("", (UserDetailModel user, IUserDetailFacade userFacade) =>
    {
        var createdUserId = userFacade.Create(user);
        return TypedResults.Ok(createdUserId);
    }).RequireAuthorization();

    // Update user details
    userEndpoints.MapPut("", (UserDetailModel user, IUserDetailFacade userFacade) => userFacade.Update(user));

    // Delete user
    userEndpoints.MapDelete("{id:guid}", (Guid id, IUserDetailFacade userFacade) => userFacade.Delete(id));
}

void UseFormEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var formEndpoints = routeGroupBuilder.MapGroup("form")
        .WithTags("form");

    // Get list of forms
    formEndpoints.MapGet("",
        (string? name, IFormListFacade facade) => name != null ? facade.SearchAllByName(name) : facade.GetAll());

    // Get form detail
    formEndpoints.MapGet("{id:guid}",
        Results<Ok<FormDetailModel>, NotFound<string>> (Guid id, IFormDetailFacade formFacade)
            => formFacade.GetById(id) is { } form
                ? TypedResults.Ok(form)
                : TypedResults.NotFound($"Form with ID {id} not found."));

    // Create new Form
    formEndpoints.MapPost("", (FormDetailModel form, IFormDetailFacade formFacade) =>
    {
        var createdFormId = formFacade.Create(form);
        return TypedResults.Ok(createdFormId);
    });

    // Update form details
    formEndpoints.MapPut("", (FormDetailModel form, IFormDetailFacade formFacade) => formFacade.Update(form));

    // Delete form
    formEndpoints.MapDelete("{id:guid}", (Guid id, IFormDetailFacade formFacade) => formFacade.Delete(id));
}

void UseCompletedFormEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var compFormEndpoints = routeGroupBuilder.MapGroup("completedForm")
        .WithTags("completedForm");

    // Get list of completed forms, by form Id or all forms completed by a user
    compFormEndpoints.MapGet("", (string? formId, string? userId, ICompletedFormListFacade facade) =>
    {
        if (formId != null)
            return facade.GetAllCopletionsOfForm(new Guid(formId));

        if (userId != null)
            return facade.GetAllCompletionsMadeByUser(new Guid(userId));

        return facade.GetAll();
    });

    // Get form detail
    compFormEndpoints.MapGet("{id:guid}",
        Results<Ok<CompletedFormDetailModel>, NotFound<string>> (Guid id, ICompletedFormDetailFacade formFacade)
            => formFacade.GetById(id) is { } form
                ? TypedResults.Ok(form)
                : TypedResults.NotFound($"Form with ID {id} not found."));

    // Create new completedForm
    compFormEndpoints.MapPost("", (CompletedFormDetailModel form, ICompletedFormDetailFacade formFacade) =>
    {
        var createdFormId = formFacade.Create(form);
        return TypedResults.Ok(createdFormId);
    });

    // Delete form
    compFormEndpoints.MapDelete("{id:guid}", (Guid id, ICompletedFormDetailFacade formFacade) => formFacade.Delete(id));
}

void UseQuestionEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var questionEndpoints = routeGroupBuilder.MapGroup("question")
        .WithTags("question");

    // Get list of questions
    questionEndpoints.MapGet("", (string? formId, string? text, string? desc, IQuestionListFacade facade) =>
    {
        Guid? formGuid = formId != null ? new Guid(formId) : null;
        return facade.GetAll(formGuid, text, desc);
    });

    // Get question detail
    questionEndpoints.MapGet("{id:guid}",
        Results<Ok<QuestionDetailModel>, NotFound<string>> (Guid id, IQuestionDetailFacade questionFacade)
            => questionFacade.GetById(id) is { } question
                ? TypedResults.Ok(question)
                : TypedResults.NotFound($"Question with ID {id} not found."));

    // Create new question
    questionEndpoints.MapPost("", (QuestionDetailModel question, IQuestionDetailFacade questionFacade) =>
    {
        var createdQuestionId = questionFacade.Create(question);
        return TypedResults.Ok(createdQuestionId);
    });

    // Update question details
    questionEndpoints.MapPut("",
        (QuestionDetailModel question, IQuestionDetailFacade questionFacade) => questionFacade.Update(question));

    // Delete question
    questionEndpoints.MapDelete("{id:guid}",
        (Guid id, IQuestionDetailFacade questionFacade) => questionFacade.Delete(id));
}

void UseAnswerEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var answerEndpoints = routeGroupBuilder.MapGroup("answer")
        .WithTags("answer");

    // Get list of answers, all, from form or from question
    answerEndpoints.MapGet("", (string? formId, string? questionId, IAnswerListFacade facade) =>
    {
        Guid? formGuid = formId != null ? new Guid(formId) : null;
        Guid? questionGuid = questionId != null ? new Guid(questionId) : null;

        return facade.GetAll(formGuid, questionGuid);
    });

    // Get answer detail
    answerEndpoints.MapGet("{id:guid}",
        Results<Ok<AnswerDetailModel>, NotFound<string>> (Guid id, IAnswerDetailFacade answerFacade)
            => answerFacade.GetById(id) is { } answer
                ? TypedResults.Ok(answer)
                : TypedResults.NotFound($"Answer with ID {id} not found."));

    // Create a new answer or update existing
    answerEndpoints.MapPost("", (AnswerSubmitModel answer, IAnswerSubmitFacade answerFacade) =>
    {
        var createdAnswerId = answerFacade.CreateOrUpdate(answer);
        return TypedResults.Ok(createdAnswerId);
    });

    // Delete answer
    answerEndpoints.MapDelete("{id:guid}", (Guid id, IAnswerDetailFacade answerFacade) => answerFacade.Delete(id));
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

void UseAuthorization(WebApplication application)
{
    application.UseAuthentication();
    application.UseAuthorization();
}

void UseOpenApi(IApplicationBuilder application)
{
    application.UseOpenApi();
    // application.UseSwaggerUi();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.OAuthClientId("your-client-id");
        c.OAuthClientSecret("your-client-secret");
        c.OAuthUsePkce();
    });
}


// Make the implicit Program class public so test projects can access it
public partial class Program
{
}
