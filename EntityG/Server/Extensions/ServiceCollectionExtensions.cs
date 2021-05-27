using EasyCaching.InMemory;
using EntityG.BusinessLogic.Caching.Interfaces.Proxies;
using EntityG.BusinessLogic.Caching.Proxies;
using EntityG.BusinessLogic.Caching.Proxies.Implements;
using EntityG.BusinessLogic.Caching.Proxies.Interfaces;
using EntityG.BusinessLogic.Configurations;
using EntityG.BusinessLogic.Interfaces.Services;
using EntityG.BusinessLogic.Interfaces.Services.Account;
using EntityG.BusinessLogic.Interfaces.Services.Identity;
using EntityG.BusinessLogic.Interfaces.Services.Shared;
using EntityG.BusinessLogic.Services;
using EntityG.BusinessLogic.Services.Account;
using EntityG.BusinessLogic.Services.Identity;
using EntityG.BusinessLogic.Services.Shared;
using EntityG.EntityFramework.Configuration.Configuration;
using EntityG.EntityFramework.Configuration.InMemory;
using EntityG.EntityFramework.Configuration.MySql;
using EntityG.EntityFramework.Configuration.PostgreSQL;
using EntityG.EntityFramework.Contexts;
using EntityG.EntityFramework.Entities.Identity;
using EntityG.EntityFramework.Interfaces;
using EntityG.EntityFramework.Interfaces.Repositories;
using EntityG.EntityFramework.Repositories;
using EntityG.EntityFramework.SqlServer.Extensions;
using EntityG.EntityFramework.UnitOfWork;
using EntityG.Server.Permission;
using EntityG.Server.Services;
using EntityG.Shared.Constants.Permission;
using EntityG.Shared.Wrapper;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace EntityG.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static AppConfiguration GetApplicationSettings(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetSection(nameof(AppConfiguration));
            services.Configure<AppConfiguration>(applicationSettingsConfiguration);

            return applicationSettingsConfiguration.Get<AppConfiguration>();
        }

        public static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //TODO - Lowercase Swagger Documents
                //c.DocumentFilter<LowercaseDocumentFilter>();
                //Refer - https://gist.github.com/rafalkasa/01d5e3b265e5aa075678e0adfd54e23f
                c.IncludeXmlComments(string.Format(@"{0}\EntityG.Server.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "EntityG",
                    License = new OpenApiLicense()
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
        }

        public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var databaseProvider = configuration.GetSection(nameof(DatabaseProviderConfiguration)).Get<DatabaseProviderConfiguration>();
           
            switch (databaseProvider.ProviderType)
            {
                case DatabaseProviderType.SqlServer:
                    services.RegisterSqlServerDbContext(configuration);
                    break;
                case DatabaseProviderType.PostgreSQL:
                    services.RegisterNpgSqlDbContext(configuration);
                    break;
                case DatabaseProviderType.MySql:
                    services.RegisterMySqlDbContext(configuration);
                    break;
                case DatabaseProviderType.InMemory:
                    services.RegisterInMemoryDbContext(configuration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseProvider.ProviderType), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DatabaseProviderType)))}.");
            }

            return services;
        }

        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseProvider = configuration.GetSection(nameof(DatabaseProviderConfiguration)).Get<DatabaseProviderConfiguration>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            switch (databaseProvider.ProviderType)
            {
                case DatabaseProviderType.SqlServer:
                    services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
                    break;
                case DatabaseProviderType.PostgreSQL:
                    services.AddHangfire(x => x.UsePostgreSqlStorage(connectionString));
                    break;
                case DatabaseProviderType.MySql:
                    throw new NotSupportedException();
                case DatabaseProviderType.InMemory:
                    services.AddHangfire(x => x.UseInMemoryStorage());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseProvider.ProviderType), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DatabaseProviderType)))}.");
            }

            services.AddHangfireServer();

            return services;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
                .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailConfiguration>(configuration.GetSection("MailConfiguration"));
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IAssetTypeRepository, AssetTypeRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ITimesheetRepository, TimesheetRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

            services.AddScoped<ITokenService, IdentityService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IAssetTypeService, AssetTypeService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITimesheetService, TimesheetService>();
            services.AddScoped<ILeaveService, LeaveService>();
            services.AddScoped<ILeaveTypeService, LeaveTypeService>();
            services.AddScoped<IAssetTypeProxy, AssetTypeProxy>();
            services.AddScoped<IEmployeeProxy, EmployeeProxy>();
            services.AddScoped<IUserProxy, UserProxy>();
            services.AddScoped<IProjectProxy, ProjectProxy>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services, AppConfiguration config)
        {
            var key = Encoding.ASCII.GetBytes(config.Secret);
            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero
                    };
                    bearer.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(Result.Fail("You are not Authorized."));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(Result.Fail("You are not authorized to access this resource."));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
            services.AddAuthorization(options =>
            {
                // Here I stored necessary permissions/roles in a constant
                foreach (var prop in typeof(Permissions).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                {
                    options.AddPolicy(prop.GetValue(null)?.ToString()!, policy => policy.RequireClaim(ApplicationClaimTypes.Permission, prop.GetValue(null).ToString()));
                }
            });
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }

        public static IServiceCollection AddAutoMaper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(BusinessLogic.Mappings.RoleProfile).Assembly);

            return services;
        }
        
        public static IServiceCollection AddEasyCaching(this IServiceCollection services)
        {
            services.AddEasyCaching(options =>
            {
                options.UseInMemory(config =>
                {
                    config.DBConfig = new InMemoryCachingOptions
                    {
                         // scan time, default value is 60s
                         ExpirationScanFrequency = 60,
                         // total count of cache items, default value is 10000
                         SizeLimit = 10000
                    };
                     // the max random second will be added to cache's expiration, default value is 120
                     config.MaxRdSecond = 120;
                     // whether enable logging, default is false
                     config.EnableLogging = false;
                     // mutex key's alive time(ms), default is 5000
                     config.LockMs = 5000;
                     // when mutex key alive, it will sleep some time, default is 300
                     config.SleepMs = 300;
                }, nameof(EntityG));

                // with a default name [mskpack]
                options.WithMessagePack();

                // with a custom name [myname]
                options.WithMessagePack("myname");

                // add some serialization settings
                options.WithMessagePack(x =>
                {
                    // If this setting is true, you should custom the resolver by yourself
                    // If this setting is false, also the default behavior, it will use ContractlessStandardResolver only
                    x.EnableCustomResolver = true;
                }, "cus");
            });

            return services;
        }

    }
}