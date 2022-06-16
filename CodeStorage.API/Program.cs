using CodeStorage.API.Identity;
using AspNetCore.Identity.MongoDbCore.Models;
using CodeStorage.Infrastructure;
using CodeStorage.Application;
using Microsoft.OpenApi.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CodeStorage.Domain.User;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUserService,UserService>();

builder.Services.AddIdentityConfiguration(builder.Configuration);

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});


var baseAddress = "http://localhost:8779/"; //builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey)?.Split(';').OrderBy(x => x.Contains("https://") ? 0 : 1).FirstOrDefault()?.Trim('/').Replace("http://+", "http://localhost") + '/';

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
                        TokenUrl = new Uri($"{baseAddress}api/users/login"),
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
