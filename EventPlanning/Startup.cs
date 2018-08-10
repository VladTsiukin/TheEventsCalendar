using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventPlanning.Data;
using EventPlanning.Models;
using EventPlanning.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EventPlanning.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace EventPlanning
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.Environment = env;

            // add environment
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // clear jwt
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            // add openId auth
            services.AddAuthentication(sharedOp =>
            {
                sharedOp.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOp.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOp.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(op =>
            {
                op.ClientId = Configuration["oidc:clientid"];
                op.ClientSecret = Configuration["oidc:clientsecret"];
                op.Authority = Configuration["oidc:authority"];

                op.ResponseType = OpenIdConnectResponseType.Code;
                op.SaveTokens = true;
                op.GetClaimsFromUserInfoEndpoint = true;
                op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true
                };
                op.Scope.Add("email");

                op.ClaimActions.MapAllExcept("aud", "iss", "iat", "nbf", "exp", "aio", "c_hash", "uti", "nonce");

                op.Events = new OpenIdConnectEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.HandleResponse();

                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        if (this.Environment.IsDevelopment())
                        {
                            // Debug only, in production do not share exceptions with the remote host.
                            return c.Response.WriteAsync(c.Exception.ToString());
                        }
                        return c.Response.WriteAsync("An error occurred processing your authentication.");
                    },
                    OnUserInformationReceived = c =>
                    {
                        // add role user
                        c.Principal.AddIdentity(new ClaimsIdentity
                            (new List<Claim>
                                {
                                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "user")
                                }));
                        // check email is verified
                        var emailVerified = c.Principal.FindFirst(claim => claim.Type == "email_verified").Value;
                        bool resEmailVerif = false;
                        bool.TryParse(emailVerified, out resEmailVerif);
                        if (!resEmailVerif)
                        {
                            return c.Response.WriteAsync("Confirm email in your google account.");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            // add https required
            services.Configure<MvcOptions>(op =>
            {
                op.Filters.Add(new RequireHttpsAttribute());
            });

            // add options service
            services.AddOptions();

            // add sendGrid options
            services.Configure<SendGridOptions>(op =>
            {
                op.SGUser = Configuration["SendGridUser"];
                op.SGKey = Configuration["SendGridKey"];
            });

            // add localization options
            services.Configure<RequestLocalizationOptions>(op =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("ru"),
                    new CultureInfo("en")
                };

                op.DefaultRequestCulture = new RequestCulture("en");
                op.SupportedCultures = supportedCultures;
                op.SupportedUICultures = supportedCultures;
                //op.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
                //{
                //    return new ProviderCultureResult("en");
                //}));
            });

            // add the localization services to the services container
            services.AddLocalization(op => op.ResourcesPath = "Resources");

            // add Sql Server
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // add Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(op => {
                    // confirm email
                    op.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc()
                // Add support for finding localized views, based on file name suffix, e.g. Index.fr.cshtml
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                // Add support for localizing strings in data annotations (e.g. validation messages) via the
                // IStringLocalizer abstractions.
                .AddDataAnnotationsLocalization()
                // add razorpage options
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Account/Manage");
                    options.Conventions.AuthorizePage("/Account/Logout");
                });

            // add XSRF-TOKEN
            services.AddAntiforgery(op => op.HeaderName = "RequestVerificationToken");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // use log
            loggerFactory.AddConsole(Configuration.GetSection("Loging"));
            loggerFactory.AddDebug();

            // use localization
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
