using System;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using LinqToDB.Mapping;
using TsDataAnnotations.Server;
using TsDataAnnotations.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using TsDataAnnotations.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHealthChecks(); // .AddCheck<ICMPHealthCheck>("ICMP");

string CorsPolicyName = "default";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(KeyVaultService.GetKeyBytes()),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("usuario",     policy => policy.RequireClaim("TsDataAnnotationsClaim", "usuario"));
    options.AddPolicy("admin",       policy => policy.RequireClaim("TsDataAnnotationsClaim", "admin"));
    options.AddPolicy("operador",    policy => policy.RequireClaim("TsDataAnnotationsClaim", "operador"));
    options.AddPolicy("programador", policy => policy.RequireClaim("TsDataAnnotationsClaim", "programador"));
    options.AddPolicy("dba",         policy => policy.RequireClaim("TsDataAnnotationsClaim", "dba"));
    options.AddPolicy("designer",    policy => policy.RequireClaim("TsDataAnnotationsClaim", "designer"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicyName,
                      builder =>
                      {
                          builder
                            .WithOrigins("https://localhost:40443",   "http://localhost:40080" ) 
                            .WithMethods(["HEADER", "GET", "PUT"]) 
                            .AllowAnyHeader()
                            .AllowCredentials(); 
                      });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLinqToDBContext<TsDataAnnotationsDb>((provider, options)
            => options.UseSQLite(@"Data Source=C:\Users\cesar\Documents\GitHub\Angular\TsDataAnnotations\TsDataAnnotations.Server\Database\TsDataAnnotations.db;")  
            .UseDefaultLogging(provider));

var app = builder.Build();

// using IServiceScope scope = app.Services.CreateScope();
// TsDataAnnotationsDb? db = scope.ServiceProvider.GetService<TsDataAnnotationsDb>();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(CorsPolicyName);

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
