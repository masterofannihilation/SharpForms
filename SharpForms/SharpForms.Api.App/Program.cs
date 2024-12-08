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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

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
        options.AddPolicy(UserRole.General.ToString(), policy =>
        {
            policy.RequireRole(UserRole.General.ToString());
        });

        options.AddPolicy(UserRole.Admin.ToString(), policy =>
        {
            policy.RequireRole(UserRole.Admin.ToString());
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
            return name != null ? facade.SearchAllByName(name) : facade.GetAll();
        }).RequireAuthorization();

    // Get user detail
    userEndpoints.MapGet("{id:guid}",
        Results<Ok<UserDetailModel>, NotFound<string>> (Guid id, IUserDetailFacade userFacade)
            => userFacade.GetById(id) is { } user
                ? TypedResults.Ok(user)
                : TypedResults.NotFound($"User with ID {id} not found.")).RequireAuthorization();

    // Create new user
    userEndpoints.MapPost("", (UserDetailModel user, IUserDetailFacade userFacade) =>
    {
        var createdUserId = userFacade.Create(user);
        return TypedResults.Ok(createdUserId);
    }).RequireAuthorization(UserRole.Admin.ToString());

    // Update user details
    userEndpoints.MapPut("", (UserDetailModel user, IUserDetailFacade userFacade) => userFacade.Update(user)).RequireAuthorization(UserRole.Admin.ToString());

    // Delete user
    userEndpoints.MapDelete("{id:guid}", (Guid id, IUserDetailFacade userFacade) => userFacade.Delete(id)).RequireAuthorization(UserRole.Admin.ToString());
}

void UseFormEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var formEndpoints = routeGroupBuilder.MapGroup("form")
        .WithTags("form");

    // Get list of forms
    formEndpoints.MapGet("",
     (string? name, IFormListFacade facade, ClaimsPrincipal userClaims) =>
     {
         var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;
         if (!Guid.TryParse(userClaims.FindFirst("userid")?.Value, out var userId))
             return new List<FormListModel>();

         if (userRole == UserRole.Admin.ToString())
             return name == null ? facade.GetAll() : facade.SearchAllByName(name);

         return name == null ? facade.GetAllCreatedBy(userId) : facade.SearchAllByNameAndCreatedBy(name, userId);
     }).RequireAuthorization();


    // Get form detail
    formEndpoints.MapGet("{id:guid}",
        Results<Ok<FormDetailModel>, NotFound<string>> (Guid id, IFormDetailFacade formFacade)
            => formFacade.GetById(id) is { } form
                ? TypedResults.Ok(form)
                : TypedResults.NotFound($"Form with ID {id} not found."));

    // Create new Form
    formEndpoints.MapPost("", (FormDetailModel form, IFormDetailFacade formFacade, ClaimsPrincipal userClaims) =>
    {
        var createdFormId = formFacade.Create(form);
        return TypedResults.Ok(createdFormId);
    }).RequireAuthorization();

    // Update form details
    formEndpoints.MapPut("", (FormDetailModel form, IFormDetailFacade formFacade, ClaimsPrincipal userClaims) =>
    {
        var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;
        if (!Guid.TryParse(userClaims.FindFirst("userid")?.Value, out var userId))
            return Results.Forbid();

        var existingForm = formFacade.GetById(form.Id);
        if (existingForm == null) return Results.NotFound($"Form with ID {form.Id} not found.");

        if (userRole == UserRole.Admin.ToString() || (existingForm.Creator != null && existingForm.Creator.Id == userId))
        {
            formFacade.Update(form);
            return Results.Ok();
        }

        return Results.Forbid();
    }).RequireAuthorization();

    // Delete form
    formEndpoints.MapDelete("{id:guid}", (Guid id, IFormDetailFacade formFacade, ClaimsPrincipal userClaims) =>
    {
        var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;
        if (!Guid.TryParse(userClaims.FindFirst("userid")?.Value, out var userId))
            return Results.Forbid();

        var existingForm = formFacade.GetById(id);
        if (existingForm == null) return Results.NotFound($"Form with ID {id} not found.");

        if (userRole == UserRole.Admin.ToString() || (existingForm.Creator != null && existingForm.Creator.Id == userId))
        {
            formFacade.Delete(id);
            return Results.Ok();
        }
        return Results.Forbid();
    }).RequireAuthorization();
}

void UseCompletedFormEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var compFormEndpoints = routeGroupBuilder.MapGroup("completedForm")
        .WithTags("completedForm");

    // Get list of completed forms, by form Id or all forms completed by a user
    compFormEndpoints.MapGet("",
     (string? formId, string? userId, ICompletedFormListFacade facade, ClaimsPrincipal userClaims) =>
     {
         var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;
         if (!Guid.TryParse(userClaims.FindFirst("userid")?.Value, out var currentUserId))
             return new List<CompletedFormListModel>();


         if (formId != null)
         {
             if (userId != null)
             {
                 if (userRole == UserRole.Admin.ToString()) return facade.GetAllCompletionsOfFormMadeByUser(Guid.Parse(formId), Guid.Parse(userId));
                 return new List<CompletedFormListModel>();
             }
             else
             {
                 if (userRole == UserRole.Admin.ToString()) return facade.GetAllCopletionsOfForm(Guid.Parse(formId));    
                 return facade.GetAllCompletionsOfFormMadeByUser(Guid.Parse(formId), currentUserId);
             }

         }
         else if (userId != null)
         {
             if (userRole == UserRole.Admin.ToString()) return facade.GetAllCompletionsMadeByUser(Guid.Parse(userId));
             return new List<CompletedFormListModel>();
         }
         else
         {
             if (userRole == UserRole.Admin.ToString()) return facade.GetAll();
             return facade.GetAllCompletionsMadeByUser(currentUserId);
         }
    }).RequireAuthorization();

    // Get form detail
    compFormEndpoints.MapGet("{id:guid}",
    Results<Ok<CompletedFormDetailModel>, NotFound<string>, StatusCodeHttpResult> (Guid id, ICompletedFormDetailFacade compFormFacade, IFormDetailFacade formFacade, ClaimsPrincipal userClaims) =>
    {
        var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;
        if (!Guid.TryParse(userClaims.FindFirst("userid")?.Value, out var currentUserId))
            return TypedResults.StatusCode(403); // Forbidden

        // Get the completed form by ID
        var form = compFormFacade.GetById(id);
        if (form == null) return TypedResults.NotFound($"Form with ID {id} not found.");

        var originalForm = formFacade.GetById(form.FormId);

        if (userRole == UserRole.Admin.ToString() || 
        (form.User != null && form.User.Id == currentUserId) || 
        (originalForm != null && originalForm.Creator != null && originalForm.Creator.Id == currentUserId))
            return TypedResults.Ok(form);

        return TypedResults.StatusCode(403); // Forbidden
    }).RequireAuthorization();

    // Create new completedForm
    compFormEndpoints.MapPost("", (CompletedFormDetailModel form, ICompletedFormDetailFacade formFacade) =>
    {
        var createdFormId = formFacade.Create(form);
        return TypedResults.Ok(createdFormId);
    });

    // Delete form
    compFormEndpoints.MapDelete("{id:guid}", (Guid id, ICompletedFormDetailFacade formFacade, ClaimsPrincipal userClaims) => 
    {
        var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;
        if (!Guid.TryParse(userClaims.FindFirst("userid")?.Value, out var currentUserId))
            return Results.Forbid();

        var form = formFacade.GetById(id);
        if (form == null) return Results.NotFound($"Form with ID {id} not found.");

        if(userRole == UserRole.Admin.ToString() || (form.User != null && form.User.Id == currentUserId))
        {
            formFacade.Delete(id);
            return Results.Ok();
        }

        return Results.Forbid();
    
    }).RequireAuthorization();
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
    questionEndpoints.MapPost("", 
        (QuestionDetailModel question, IQuestionDetailFacade questionFacade, IFormDetailFacade formFacade, ClaimsPrincipal userClaims) =>
    {
        var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;
        if (!Guid.TryParse(userClaims.FindFirst("userid")?.Value, out var currentUserId))
            return Results.Forbid();

        var form = formFacade.GetById(question.FormId);

        if(userRole == UserRole.Admin.ToString() || (form != null && form.Creator != null && form.Creator.Id == currentUserId))
        {
            var createdQuestionId = questionFacade.Create(question);
            return TypedResults.Ok(createdQuestionId);
        }
        return Results.Forbid();

    }).RequireAuthorization();

    // Update question details
    questionEndpoints.MapPut("",
        (QuestionDetailModel question, IQuestionDetailFacade questionFacade, IFormDetailFacade formFacade, ClaimsPrincipal userClaims) =>
    {
        var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;
        if (!Guid.TryParse(userClaims.FindFirst("userid")?.Value, out var currentUserId))
            return Results.Forbid();

        var form = formFacade.GetById(question.FormId);

        if (userRole == UserRole.Admin.ToString() || (form != null && form.Creator != null && form.Creator.Id == currentUserId))
        {
            var createdQuestionId = questionFacade.Update(question);
            return TypedResults.Ok(createdQuestionId);
        }
        return Results.Forbid();

    }).RequireAuthorization();

    // Delete question
    questionEndpoints.MapDelete("{id:guid}",
        (Guid id, IQuestionDetailFacade questionFacade, IFormDetailFacade formFacade, ClaimsPrincipal userClaims) =>
        {
            var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;
            if (!Guid.TryParse(userClaims.FindFirst("userid")?.Value, out var currentUserId))
                return Results.Forbid();

            var question = questionFacade.GetById(id);
            if (question == null) return Results.NotFound($"Question with ID {id} not found.");
            var form = formFacade.GetById(question.FormId);

            if (userRole == UserRole.Admin.ToString() || (form != null && form.Creator != null && form.Creator.Id == currentUserId))
            {
                questionFacade.Delete(id);
                return Results.Ok();
            }
            return Results.Forbid();

        }).RequireAuthorization();
}

void UseAnswerEndpoints(RouteGroupBuilder routeGroupBuilder)
{
    var answerEndpoints = routeGroupBuilder.MapGroup("answer")
        .WithTags("answer");

    // Get list of answers, all, from form or from question
    answerEndpoints.MapGet("", 
        (string? formId, string? questionId, IAnswerListFacade answerFacade, IFormDetailFacade formFacade, IQuestionDetailFacade questionFacade, ClaimsPrincipal userClaims) =>
    {
        var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;
        if (! Guid.TryParse(userClaims.FindFirst("userid")?.Value, out var currentUserId))
            return new List<AnswerListModel>();
        
        FormDetailModel? form = null;
        QuestionDetailModel? question = null;
        Guid? form_Id = formId != null ? Guid.Parse(formId) : null;
        Guid? question_Id = questionId != null ? Guid.Parse(questionId) : null;

        if (formId != null) form = formFacade.GetById(Guid.Parse(formId));
        
        else if(questionId != null)
        {
            question = questionFacade.GetById(Guid.Parse(questionId));
            if (question != null) form = formFacade.GetById(question.FormId);
        }
        
        if(userRole == UserRole.Admin.ToString() || 
        (form != null && form.Creator != null && form.Creator.Id == currentUserId))
        {
            return answerFacade.GetAll(form_Id, question_Id);
        }
        
        return new List<AnswerListModel>();

    }).RequireAuthorization();

    // Get answer detail
    answerEndpoints.MapGet("{id:guid}",
        Results<Ok<AnswerDetailModel>, NotFound<string>, StatusCodeHttpResult> 
        (Guid id, IAnswerDetailFacade answerFacade, IQuestionDetailFacade questionFacade, IFormDetailFacade formFacade, ClaimsPrincipal userClaims) => 
        {
            var userRole = userClaims.FindFirst(ClaimTypes.Role)?.Value;
            if (!Guid.TryParse(userClaims.FindFirst("userid")?.Value, out var currentUserId))
                return TypedResults.StatusCode(403); // Forbidden

        AnswerDetailModel? answer = answerFacade.GetById(id);
            if (answer == null) return TypedResults.NotFound($"Answer with ID {id} not found.");

            FormDetailModel? form = null;

            if(answer.Question != null)
            {
                var question = questionFacade.GetById(answer.Question.Id);
                if (question != null) form = formFacade.GetById(question.FormId);
            }

        if (userRole == UserRole.Admin.ToString() || 
            (form != null && form.Creator != null && form.Creator.Id == currentUserId) || 
            (answer.User != null && answer.User.Id == currentUserId))
            {
                var answerDetail = answerFacade.GetById(id);
                if (answerDetail == null) return TypedResults.NotFound($"Answer with ID {id} not found.");
                return TypedResults.Ok(answerDetail);
            }
        else return TypedResults.StatusCode(403); // Forbidden

        }).RequireAuthorization();

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
