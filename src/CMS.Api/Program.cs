using CMS.Business.Consumers;
using CMS.Business.Helper;
using CMS.Business.Infrastructure;
using CMS.Business.Infrastructure.Middlewares;
using CMS.Business.Middlewares;
using CMS.Business.Services;
using CMS.Business.Validations;
using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Storage.Consts;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Data;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<BaseValidator>();

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase)
.AddOData(action =>
{
    action.EnableQueryFeatures();
});

builder.Services.AddDbContext<CMSContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("AppDb"),
       options =>
       {
           options.EnableRetryOnFailure();
       }));

Global.Initialize(builder.Configuration);

var key = Encoding.ASCII.GetBytes(Global.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();
builder.Services.UseCustomValidationMiddleware();

#region Dependencies

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<IAccessRightService, AccessRightService>();
builder.Services.AddScoped<IContactCategoryService, ContactCategoryService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatMessageService, ChatMessageService>();
builder.Services.AddScoped<IAccessRightService, AccessRightService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IBlogCategoryService, BlogCategoryService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ITaskCategoryService, TaskCategoryService>();
builder.Services.AddScoped<ITaskStatusService, TaskStatusService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IMailHelper, MailHelper>();
builder.Services.AddScoped<IWebsiteParameterService, WebsiteParameterService>();
builder.Services.AddScoped<IMailTemplateService, MailTemplateService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITestimonialService, TestimonialService>();
builder.Services.AddScoped<IService_Service, Service_Service>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IHomepageSliderService, HomepageSliderService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IUserJobService, UserJobService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IAccessRightEndpointService, AccessRightEndpointService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<RequestLocalizationCookiesMiddleware>();
builder.Services.AddScoped<IUserFileService, UserFileService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserTokenService, UserTokenService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<IAccessRightCategoryService, AccessRightCategoryService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ILanguageValueService, LanguageValueService>();
builder.Services.AddScoped<ILanguageHelper, LanguageHelper>();

#endregion

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SendEmailConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint(RabbitMQEndpoint.EmailQueue, e =>
        {
            e.ConfigureConsumer<SendEmailConsumer>(context);
        });
    });
});

builder.Services.AddScoped<IDbConnection>(sp =>
{
    return new SqlConnection(builder.Configuration.GetConnectionString("AppDb"));
});

var app = builder.Build();

app.MapOpenApi();

app.MapScalarApiReference();

string[] corsDomains = builder.Configuration["CorsDomains"].Split(",");

app.UseCors(options => options.WithOrigins(corsDomains)
                              .AllowAnyMethod()
                              .AllowCredentials()
                              .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
}

app.UseAuthentication();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandleMiddleware();

app.Run();