using CMS.Data.Context;
using CMS.Data.Repository;
using CMS.Model.Model;
using CMS.Service;
using CMS.Service.Helper;
using CMS.Service.Infrastructure;
using CMS.Service.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;

namespace CMS.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<CMSContext>(options => options.UseSqlite(Configuration.GetConnectionString("AppConnectionString")));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            //services.AddScoped(typeof(IWebsiteParameterService<>), typeof(WebsiteParameterService<>));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtHelper, JwtHelper>();
            services.AddScoped<IAccessRightService, AccessRightService>();
            services.AddScoped<IContactCategoryService, ContactCategoryService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IChatMessageService, ChatMessageService>();
            services.AddScoped<IUserAccessRightService, UserAccessRightService>();
            services.AddScoped<IAccessRightService, AccessRightService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IBlogCategoryService, BlogCategoryService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<ITodoCategoryService, TodoCategoryService>();
            services.AddScoped<ITodoStatusService, TodoStatusService>();
            services.AddScoped<ITodoService, TodoService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IMailHelper, MailHelper>();
            services.AddScoped<IWebsiteParameterService, WebsiteParameterService>();
            services.AddScoped<IMailTemplateService, MailTemplateService>();

            services.ConfigureApplicationCookie(s =>
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

            services.AddSession();
            services.AddSignalR();
            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
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
                                StatusCode = (int)HttpStatusCode.BadRequest
                            });
                    };
                }).AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            services.AddMemoryCache();

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ErrorHandler();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            Global.Initialize(Configuration);

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
