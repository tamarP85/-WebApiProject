using Microsoft.OpenApi.Models;
using WebApiProject.Middleware;
using WebApiProject.Services;
var builder = WebApplication.CreateBuilder(args);

// הוספת שירותי ה-Controllers
builder.Services.AddControllers();
// builder.Services.AddIceCreamConst();
builder.Services.AddIceCreamJson();
builder.Services.AddUserJson();

// הוספת Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
var app = builder.Build();

// קביעת המידלוואר לשרת את Swagger כנקודת JSON
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}
app.UseLogMiddleware();
app.UseErrorMiddleware();
// app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

app.Run();


