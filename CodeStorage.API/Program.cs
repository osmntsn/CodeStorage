using Microsoft.IdentityModel.Tokens;
using CodeStorage.API.Identity;
using AspNetCore.Identity.MongoDbCore.Models;
using IdentityServer4.Models;
using CodeStorage.Infrastructure;
using CodeStorage.Application;
using Microsoft.OpenApi.Models;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddInMemoryClients(new List<Client>())
            .AddInMemoryApiScopes(new List<ApiScope>())
            .AddAspNetIdentity<ApplicationUser>();

var baseAddress = "http://localhost:8779/"; //builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey)?.Split(';').OrderBy(x => x.Contains("https://") ? 0 : 1).FirstOrDefault()?.Trim('/').Replace("http://+", "http://localhost") + '/';
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer",
             options =>
             {
                 options.Authority = baseAddress;
                 options.Audience = baseAddress;
                 options.RequireHttpsMetadata = false;

                 options.TokenValidationParameters = new
                  TokenValidationParameters()
                 {
                     ValidateAudience = false
                 };
             });

builder.Services.AddSwaggerGen(
    c =>
    {
        c.AddSecurityDefinition(
            "OAuth2",
            new OpenApiSecurityScheme
            {
                Flows = new OpenApiOAuthFlows
                {

                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        Scopes = new Dictionary<string, string>
                        {
                            ["remote.api.read"] = "remote.api.read",
                            ["remote.api.write"] = "remote.api.write"
                        },
                        TokenUrl = new Uri($"{baseAddress}connect/token"),
                    },
                },
                In = ParameterLocation.Header,
                Name = HeaderNames.Authorization,
                Type = SecuritySchemeType.OAuth2,
            }
        );
        c.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                            {
                                new OpenApiSecurityScheme
                                {

                                    Reference = new OpenApiReference
                                        { Type = ReferenceType.SecurityScheme, Id = "OAuth2" },
                                },
                                new[] { "remote.api" }
                            }
            }
        );
    });

var app = builder.Build();

app.UseIdentityServer();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
