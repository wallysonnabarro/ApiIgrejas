using Domain.Dominio;
using Infra.CrossCutting.IoC;
using Infra.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.UTF8.GetBytes(Settings.Secret);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

AddSwagger(builder);

builder.Services.AddDbContext<ContextDb>(options =>
{
    options.UseSqlServer(connection);
});

builder.Services.AddDbContext<ContextDb>(options =>
                    options.UseSqlServer(
                    builder.Configuration.GetConnectionString(connection!)), ServiceLifetime.Scoped);

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
        ValidateAudience = false,
        ValidateLifetime = true
    };
});

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://bracofortedosenhorsiao.azurewebsites.net")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

builder.Services.AddAntiforgery(options => {
    options.HeaderName = "X-XSRF-TOKEN";
});

RegisterServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();

static void RegisterServices(IServiceCollection services)
{
    NativeInjectorBootStrappercs.RegisterServices(services);
}

static void AddSwagger(WebApplicationBuilder builder)
{
    builder.Services.AddSwaggerGen(s =>
    {
        s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Version = "v1",
            Title = "Sistema de gestão e controle de igrejas - WebApi",
            Description = "Este sistema tem a finalizade de gerenciar o controle financeiro das igrejas - WebApi",
            //Contact = new Microsoft.OpenApi.Models.OpenApiContact
            //{
            //    Name = "Sala da cidadania",
            //    Url = new Uri("https://saladacidadania.incra.gov.br/")
            //}
        });

        s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorisation",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT authentication for minimal API"
        });

        s.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[]{}
            }
        });

        string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        if (File.Exists(xmlPath))
        {
            s.IncludeXmlComments(xmlPath);
        }
    });
}