using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 10;
                option.Password.RequiredUniqueChars = 3;
                option.Password.RequireNonAlphanumeric = false;
                option.SignIn.RequireConfirmedEmail = false;
                option.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                option.Lockout.MaxFailedAccessAttempts = 3;
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation")
            ;

            services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(5));
            services.Configure<CustomEmailConfirmationTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromDays(3));

            services.AddMvc(option =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                option.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "909546360060-fuc10gnba9tng20c0r06bs6n8mbs9lti.apps.googleusercontent.com";
                options.ClientSecret = "5B3eSceMmvoYdXM1XEYlSfLy";
                //options.CallbackPath = "";
            })
            .AddFacebook(options =>
            {
                options.AppId = "3356097987746983";
                options.AppSecret = "23816369df53e55e51af53595d5e7e05";
            });

            services.ConfigureApplicationCookie(option =>
            {
                option.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

            services.AddAuthorization(option =>
            {
                option.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));

                option.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
                //option.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role", "true"));
                //option.AddPolicy("EditRolePolicy", policy => policy.RequireAssertion(context => context.User.IsInRole("Admin") &&
                //                                  context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") ||
                //                                  context.User.IsInRole("Super Admin")
                //                                  ));
                option.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));
            });

            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
            services.AddSingleton<DataProtectionPurposeStrings>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions
                //{
                //    SourceCodeLineCount = 1
                //};
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }

            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("foo.html");
            //app.UseDefaultFiles(defaultFiles);
            app.UseStaticFiles();
            //app.UseMvcWithDefaultRoute();
            app.UseAuthentication();
            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
            //app.UseMvc();
            //app.UseFileServer(fileServerOptions);
            //app.UseFileServer();
            //app.Use(async (context, next) =>
            //{ 
            //    logger.LogInformation("MW1: Incoming Request");
            //    //await context.Response.WriteAsync("Hello from 1st middleware");
            //    await next();
            //    logger.LogInformation("MW1: Outgoing Response");
            //});

            //app.Use(async (context, next) =>
            //{
            //    logger.LogInformation("MW2: Incoming Request");
            //    //await context.Response.WriteAsync("Hello from 1st middleware");
            //    await next();
            //    logger.LogInformation("MW2: Outgoing Response");
            //});

            //app.Run(async (context) =>
            //{
            //    //throw new Exception("Error processing the request");
            //    await context.Response.WriteAsync("Hello world ");
            //    //logger.LogInformation("MW3: Request handled and Response produced");
            //});
        }
    }
}
