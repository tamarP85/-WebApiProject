// // using Microsoft.OpenApi.Models;
// // using WebApiProject.Middleware;
// // using WebApiProject.Services;
// // using WebApiProject.Models;
// // using Services; // ודא שזהו ה-namespace הנכון

// // var builder = WebApplication.CreateBuilder(args);

// // // הוספת שירותי ה-Controllers
// // builder.Services.AddControllers();
// // builder.Services.AddGenericJson<IceCream,IceCreamServiceJson>();
// // builder.Services.AddGenericJson<User,UserServiceJson>();
// // builder.Services.AddCustomAuthentication(builder.Configuration); // שינוי: הוספת קריאה ל-AddCustomAuthentication

// // // הוספת Swagger
// //   builder.Services.AddSwaggerGen(c =>
// //             {
// //                 c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
// //                 c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
// //                 {
// //                     In = ParameterLocation.Header,
// //                     Description = "Please enter JWT with Bearer into field",
// //                     Name = "Authorization",
// //                     Type = SecuritySchemeType.ApiKey
// //                 });
// //                 c.AddSecurityRequirement(new OpenApiSecurityRequirement {
// //                 {
// //                      new OpenApiSecurityScheme
// //                         {
// //                          Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
// //                         },
// //                     new string[] {}
// //                 }
// //                 });
// //             });
// // var app = builder.Build();

// // // קביעת המידלוואר לשרת את Swagger כנקודת JSON
// // if (app.Environment.IsDevelopment())
// // {
// //     app.UseSwagger();
// //     app.UseSwaggerUI();
    
// // }
// // app.UseAuthentication(); // חובה לפני Authorization
// // app.UseAuthorization();
// // app.UseLogMiddleware();
// // app.UseErrorMiddleware();
// // // app.UseHttpsRedirection();
// // app.UseDefaultFiles();
// // app.UseStaticFiles();
// // // app.UseAuthorization();
// // app.MapControllers();

// // app.Run();

using Microsoft.OpenApi.Models;
using WebApiProject.Middleware;
using WebApiProject.Services;
using WebApiProject.Models;

// ודא שזהו ה-namespace הנכון
using Services;

var builder = WebApplication.CreateBuilder(args);

// הוספת שירותי ה-Controllers
builder.Services.AddControllers();
builder.Services.AddGenericJson<IceCream, IceCreamServiceJson>();
builder.Services.AddGenericJson<User, UserServiceJson>();
builder.Services.AddCustomAuthentication(builder.Configuration); // הוספת קריאה ל-AddCustomAuthentication

// הוספת Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization", // שונה מ-Alice ל-Authorization
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

var app = builder.Build();

// קביעת המידלוואר לשרת את Swagger כנקודת JSON
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseAuthentication(); // חובה לפני Authorization
app.UseAuthorization();
app.UseLogMiddleware();
app.UseErrorMiddleware();
// app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();

