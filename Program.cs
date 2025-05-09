
using Microsoft.OpenApi.Models;
using WebApiProject.Middleware;
using WebApiProject.Middlewares;
using WebApiProject.Services;
using WebApiProject.Models;
using Serilog; 
using Services;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console() 
    .WriteTo.File("logs/", rollingInterval: RollingInterval.Day,
    fileSizeLimitBytes:100_000_000,
    retainedFileCountLimit:30) 
    .CreateLogger();

builder.Host.UseSerilog();


builder.Services.AddControllers();
builder.Services.AddGenericJson<IceCream, IceCreamServiceJson>();
builder.Services.AddGenericJson<User, UserServiceJson>();
builder.Services.AddCustomAuthentication(builder.Configuration); 


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization", 
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});


builder.Services.AddScoped<IceCreamServiceJson>(); 
builder.Services.AddScoped<UserServiceJson>(); 
builder.Services.AddScoped<ActiveUserService>(); 

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseAuthentication();
app.UseMiddleware<ActiveUserMiddleware>(); 
app.UseAuthorization();
app.UseMiddleware<ActiveUserMiddleware>(); 
app.UseLogMiddleware();
app.UseErrorMiddleware();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();

app.Run();
