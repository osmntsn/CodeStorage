using Microsoft.IdentityModel.Tokens;
using CodeStorage.API.Identity;
using AspNetCore.Identity.MongoDbCore.Models;
using IdentityServer4.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddInMemoryClients(new List<Client>())
            .AddInMemoryApiScopes(new List<ApiScope>())
            .AddAspNetIdentity<ApplicationUser>();

var baseAddress = builder.WebHost.GetSetting(WebHostDefaults.ServerUrlsKey)?.Split(';').OrderBy(x => x.Contains("https://") ? 0 : 1).FirstOrDefault()?.Trim('/') + '/';
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
