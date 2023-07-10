using MRE;
using MRE.Api.Profiles;
using MRE.Application.Behaviors;
using MRE.Application.Features.AuthFeatures.Queries;
using MRE.Application.Features.AuthFeatures.Validators;
using MRE.Application.Features.UserFeatures.Commands;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Presistence.Abstruct;
using MRE.Presistence.Concrete;
using MRE.Presistence.Context;
using MRE.Presistence.Extensions;
using MRE.Presistence.IProvider;
using MRE.Presistence.IProviders;
using MRE.Presistence.Providers;
using MRE.Presistence.Seed;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Model.Models;
using Newtonsoft.Json;
using Serilog;
using System.Reflection;
using System.Text;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
//Serilog
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .WriteTo.Stackify()
  .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging();
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Auth:SecretKey"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };

});

builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
builder.Services.AddScoped<ISeedProvider, SeedProvider>();
builder.Services.AddScoped<IAzureStorageProvider, AzureStorageProvider>();
builder.Services.AddScoped<IAuthProvider, AuthProvider>();
builder.Services.AddScoped<IEmailProvider, EmailProvider>();

builder.Services.AddSingleton<IPdfProvider, PdfProvider>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ILookupRepository, LookupRepository>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
builder.Services.AddProjectConfig();


builder.Services.AddTransient<DataContext>();


builder.Services.UseDinkToPdf();

builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ConfigModel>(builder.Configuration.GetSection("Config"));
builder.Services.Configure<AuthSettingsModel>(builder.Configuration.GetSection("Auth"));
builder.Services.Configure<List<SuperAdminUserModel>>(builder.Configuration.GetSection("SuperAdminUsers"));
builder.Services.AddOptions();

Assembly[] assemblyArr = { typeof(LoginQuery).GetTypeInfo().Assembly, typeof(CreateUserCommand).GetTypeInfo().Assembly };
builder.Services.AddMediatR(assemblyArr);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


//// Add Hangfire services.
//builder.AddHangFire(connectionString);

//// Add the processing server as IHostedService
//builder.Services.AddHangfireServer();
builder.Services.AddValidatorsFromAssemblyContaining<LoginQueryValidator>();
builder.Services.AddControllers(config =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
}).AddNewtonsoftJson(ele =>
{
    ele.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("web", new OpenApiInfo { Title = "MRE - V1", Version = "web" });
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    config.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    config.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement();
    securityRequirement.Add(securitySchema, new[] { "Bearer" });
    config.AddSecurityRequirement(securityRequirement);

    config.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    ExceptionHandler = async context =>
    {
        var provider = builder.Services.BuildServiceProvider();
        var logger = provider.GetService<ILogger<IHostBuilder>>();

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature.Error;
        logger.LogInformation(exception, "Exception Occured...");
        var errorModel = new CqrsResponse()
        {
            StatusCode = System.Net.HttpStatusCode.BadRequest,
        };

        if (exception is AggregateException)
        {
            var exArr = exception as AggregateException;
            errorModel.ErrorMessage = exArr.InnerExceptions.Select(x => x.Message).FirstOrDefault();

        }
        else
        {
            errorModel.ErrorMessage = exception.Message;
        }

        var result = JsonConvert.SerializeObject(errorModel);
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    }
});
app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.SwaggerEndpoint("/swagger/web/swagger.json", "MRE For Web - V1");
});

app.UseCors("corsapp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
//app.UseHangFire();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var hostingEnvironment = services.GetService<IWebHostEnvironment>();
    var seed = services.GetRequiredService<ISeedProvider>();

    try
    {
        if (hostingEnvironment.IsDevelopment())
        {
            seed.InitDevelopment();
        }
        else
        {
            seed.InitProduction();
        }

    }
    catch (Exception ex)
    {
        throw ex;
    }
}

app.Run();


