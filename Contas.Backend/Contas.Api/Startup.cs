using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Contas.Api
{
    using Contas.Core.Extensions;
    using Contas.Core.Handlers;
    using Contas.Core.Models;
    using Contas.Core.Services.Interfaces;
    using Contas.Core.ViewModels;
    using Microsoft.AspNetCore.Identity;

    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // add migration: dotnet ef migrations add tabelaInformativos --project ./Contas.Core --startup-project ./Contas.Api
            // update database: dotnet ef database update --project ./Contas.Core --startup-project ./Contas.Api
            services.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(Configuration.GetConnectionStringEnv("DefaultConnection")));

            services.Configure<ServiceSettings>(this.Configuration.GetSectionEnv("ServiceSettings"));

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            //Faz a injeção de dependencias de forma automática, caso as interfaces estiverem nas pastas corretas
            Assembly ass = typeof(IAutenticacaoServices).GetTypeInfo().Assembly;
            string pathCore = "Contas.Core.Services";
            var allServices = ass.GetTypes().Where(t => (t.Namespace != null && t.Namespace.Equals($"{pathCore}.Commands") && t.IsClass == true));

            foreach (var type in allServices)
            {
                var allInterfaces = type.GetInterfaces().Where(t => (t.Namespace != null && t.Namespace.Equals($"{pathCore}.Interfaces")));
                var mainInterfaces = allInterfaces.Except
                        (allInterfaces.SelectMany(t => t.GetInterfaces()));
                foreach (var itype in mainInterfaces)
                {
                    if (allServices.Any(x => !x.Equals(type) && itype.IsAssignableFrom(x)))
                    {
                        throw new Exception("The " + itype.Name + " type has more than one implementations, please change your filter");
                    }
                    services.AddTransient(itype, type);
                }
            }

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            services.AddMvc(options =>
            {
                //options.Filters.Add(new ActionFilterAsync());
            }).AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSingleton<ILogService, LogService>();

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(Configuration.GetSectionEnv("TokenConfigurations")).Configure(tokenConfigurations);

            services.Configure<EmailConfigurations>(Configuration.GetSectionEnv("EmailSettings"));

            services.Configure<TokenConfigurations>(Configuration.GetSectionEnv("TokenConfigurations"));

            services.Configure<SwaggerConfiguration>(Configuration.GetSectionEnv("SwaggerSettings"));

            services.AddSingleton(tokenConfigurations);
            var key = Encoding.ASCII.GetBytes(tokenConfigurations.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
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

            services.AddAuthorization();

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddMemoryCache();

            services.AddCors();

            services.Configure<RequestLocalizationOptions>(
                   opts =>
                   {
                       var supportedCultures = new[] { new CultureInfo("pt-BR") };

                       opts.DefaultRequestCulture = new RequestCulture("pt-BR");
                       // Formatting numbers, dates, etc.
                       opts.SupportedCultures = supportedCultures;
                       // UI strings that we have localized.
                       opts.SupportedUICultures = supportedCultures;

                       
                   });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Contas API",
                    Description = "API de acesso aos recursos do Beep",
                    Contact = new OpenApiContact
                    {
                        Name = "Administrador",
                        Email = "contas@contas.com.br",
                        Url = new Uri("https://contas.com.br")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Licença de uso",
                        Url = new Uri("https://contas.com.br/license")
                    }
                });

                c.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement() { { new OpenApiSecurityScheme {
                    Reference = new OpenApiReference
                      {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                      },
                      Scheme = "oauth2",
                      Name = "Bearer",
                      In = ParameterLocation.Header,
                    },
                    new List<string>()
                  }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (!File.Exists(xmlPath))
                    File.Create(xmlPath);

                c.IncludeXmlComments(xmlPath);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
        {
            // migrate database changes on startup (includes initial db creation)
            context.Database.Migrate();

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR")
            });
            var cultureInfo = new CultureInfo("pt-BR");
            cultureInfo.NumberFormat.CurrencySymbol = "R$";

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseSwaggerAuthorized();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contas API V1");
                c.RoutePrefix = string.Empty;

            });
                        
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
