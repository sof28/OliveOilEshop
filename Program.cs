using FluentValidation;
using ModularCommerce.Exceptions;
using ModularCommerce.Extensions;
using ModularCommerce.Features.OrderFeature.CreateOrder;
using ModularCommerce.Repository;
using Scalar.AspNetCore;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSQLDatabaseConfiguration(builder.Configuration);

builder.Services.RegisterApiEndpointsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddHealthChecksConfiguration();

builder.Services.AddValidatorsFromAssembly(typeof(CreateOrderValidator).Assembly);

builder.Services.AddHandlersFromAssembly(typeof(Program).Assembly);

builder.Services.AddExceptionHandler<CustomExceptionHandler>()
            .AddProblemDetails();

// JWT Authentication configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});
builder.Services.AddAuthorization();

// Add CORS policy for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClientApp",
        policy => policy
            .WithOrigins("http://localhost:4200") // Angular dev server default
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

var app = builder.Build();

app.MapApiEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHealthChecks();


app.MapScalarApiReference(options =>
{
    options.WithTheme(ScalarTheme.DeepSpace);

    // ?? THIS enables Developer Tools (request + client code panel)
    options.WithDefaultHttpClient(
        ScalarTarget.CSharp,
        ScalarClient.HttpClient);

});

app.UseCors("AllowClientApp"); // Enable CORS for Angular frontend
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

await app.SeedProductsAsync();

app.Run();

