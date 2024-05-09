using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Service;
using CMS.Service.Extensions;
using CMS.Service.Helper;
using CMS.Service.Infrastructure;
using CMS.Service.Middlewares;
using CMS.Storage.Model;
using CMS.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddControllers();

builder.Services.AddDbContext<CMSContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnectionString"),
       options =>
       {
           options.EnableRetryOnFailure();
       }));

builder.Services.ConfigureApplicationCookie(s =>
{
    s.LoginPath = new PathString("/login");
    s.Cookie = new CookieBuilder
    {
        Name = "cms",
        HttpOnly = false,
        Expiration = TimeSpan.FromMinutes(2),
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.Always
    };
    s.SlidingExpiration = true;
    s.ExpireTimeSpan = TimeSpan.FromMinutes(2);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddSingleton<LanguageService>();

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
builder.Services.AddScoped<IAuthorService, AuthorService>();
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

builder.Services.AddMvc().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = (context) =>
        {
            var errors = context.ModelState.Values
                                .SelectMany(x => x.Errors
                                .Select(p => p.ErrorMessage))
                                .ToList();

            return new BadRequestObjectResult(
                new BaseResult
                {
                    Message = errors.First(),
                    StatusCode = HttpStatusCode.BadRequest
                });
        };
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddControllersWithViews().AddViewLocalization();

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.AddMvc()
   .AddViewLocalization().AddDataAnnotationsLocalization(options => {
       options.DataAnnotationLocalizerProvider = (type, factory) => {
           var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
           return factory.Create("SharedResource", assemblyName.Name);
       };
   });

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(culture: "tr-TR", uiCulture: "tr-TR");

    CultureInfo[] cultures = new CultureInfo[]
    {
        new("tr-TR"),
        new("en-US")
    };

    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

string[] corsDomains = builder.Configuration["CorsDomains"].Split(",");
app.UseCors(options => options.WithOrigins(corsDomains)
                              .AllowAnyMethod()
                              .AllowCredentials()
                              .AllowAnyHeader());

app.UseRouting();

app.ErrorHandler();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization();
app.UseRequestLocalizationCookies();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

Global.Initialize(builder.Configuration);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
