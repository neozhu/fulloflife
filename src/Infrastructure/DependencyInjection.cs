// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using FluentValidation.AspNetCore;
using NSwag;
using NSwag.Generation.Processors.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace CleanArchitecture.Razor.Infrastructure;

public static class DependencyInjection
{
    public static object Configuration { get; private set; }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("CleanArchitecture.RazorDb")
                );
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))

                );
            services.AddDatabaseDeveloperPageExceptionFilter();
        }


        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });
        services.AddSingleton(s => s.GetRequiredService<IOptions<SmartSettings>>().Value);
        services.Configure<SmartSettings>(configuration.GetSection(SmartSettings.SectionName));
        services.Configure<AppConfigurationSettings>(configuration.GetSection("AppConfigurationSettings"));
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        services.Configure<QiniuSettings>(configuration.GetSection("QiniuSettings"));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


        services.AddScoped<IDictionaryService, DictionaryService>();
        services.AddScoped<IDateTime, DateTimeService>();
        services.AddTransient<IExcelService, ExcelService>();
        services.AddTransient<IQiniuService, QiniuService>();
        services.AddTransient<IUploadService, UploadService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IMailService, SMTPMailService>();
       
        services.AddAuthentication().AddCookie(options =>
        {
            options.LoginPath = "/Identity/Account/Login";
            options.LogoutPath = "/Identity/Account/Logout";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var key = configuration.GetSection("AppConfigurationSettings:Secret").Value;
            options.RequireHttpsMetadata = false;
            options.SaveToken = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key)),
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Headers.ContainsKey("Authorization"))
                    {
                        var bearerstr = context.Request.Headers["Authorization"].FirstOrDefault(k => k.StartsWith("Bearer"));
                        if (!string.IsNullOrEmpty(bearerstr))
                        {
                            var keyval = bearerstr.Split(" ");
                            if (keyval != null && keyval.Length > 1)
                            {
                                context.Token = keyval[1];
                            }
                        }
                    }
                    return Task.CompletedTask;
                }
            };
        });
        services.Configure<IdentityOptions>(options =>
        {
            // Default SignIn settings.
            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            // Default Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        });
        services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2));
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            // Here I stored necessary permissions/roles in a constant
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()));
                }
            }
        });
        services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationClaimsIdentityFactory>();
        // Localization
        services.AddLocalization(options => options.ResourcesPath = LocalizationConstants.ResourcesPath);
        services.AddScoped<LocalizationCookiesMiddleware>();
        services.AddScoped<ExceptionMiddleware>();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.AddSupportedUICultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray());
            options.FallBackToParentUICultures = true;

        });
        services.AddControllers();
        services.AddSignalR();
        services.AddRazorPages(options =>
                 {
                     options.Conventions.AddPageRoute("/AspNetCore/Welcome", "");
                 })
                .AddFluentValidation(fv =>
                 {
                     fv.DisableDataAnnotationsValidation = true;
                     fv.ImplicitlyValidateChildProperties = true;
                     fv.ImplicitlyValidateRootCollectionElements = true;
                 })
                 .AddViewLocalization()
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.PropertyNamingPolicy = null;

                 });

        services.AddOpenApiDocument(configure =>
        {
            configure.Title = "FullOfLife API";
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });
            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });


        return services;
    }



}
