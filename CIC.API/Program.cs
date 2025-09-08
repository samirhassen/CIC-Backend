using CIC.API.Repository;
using CIC.API.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var config = builder.Configuration;
var tenantId = config["AzureAd:TenantId"];
var audience = config["AzureAd:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://login.microsoftonline.com/{tenantId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://sts.windows.net/{tenantId}/",

            ValidateAudience = true,
            ValidAudience = audience,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

// Authorization
builder.Services.AddAuthorization();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CIC.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new List<string>()
        }
    });
});

// Scoped services
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IPowerBIService, PowerBIService>();
builder.Services.AddScoped<CRM4MServiceReference.AuthenticationWebServiceSoap>(sp =>
{
    var client = new CRM4MServiceReference.AuthenticationWebServiceSoapClient(
        CRM4MServiceReference.AuthenticationWebServiceSoapClient.EndpointConfiguration.Authentication_x0020_Web_x0020_ServiceSoap
    );
    client.Endpoint.Address = new System.ServiceModel.EndpointAddress("https://cicstagednn1.pcsbcloud.com/DesktopModules/MX/Authentication.asmx");
    return client;
});
builder.Services.AddScoped<ICICCRM4MAuthenticationService,CICCRM4MAuthenticationService>();
builder.Services.AddScoped<HttpClient>();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CIC.API V1");
        options.RoutePrefix = string.Empty;
    });
}

// Middleware
app.UseCors("AllowAngular");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/api/secure-data", () => "This is protected API").RequireAuthorization();
app.MapControllers();
app.Run();
