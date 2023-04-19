using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//documentation / documentacion y setteo de swagger
builder.Services.AddSwaggerGen(c =>{
    // openapi service reference config.
    c.CustomOperationIds(a =>
    {
        return a.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });

    // para los comentarios en el swagger
    var archivoXML = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var rutaXML = System.IO.Path.Combine(AppContext.BaseDirectory, archivoXML);
    c.IncludeXmlComments(rutaXML);


    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Test para Edge Jobs",
        Version = "v1",
        Description = "API REST utilizando JsonPlaceHolder",
        Contact = new OpenApiContact
        {
            Email = "trojan.v6@gmail.com",
            Name = "Sergio Britos",
            Url = new Uri("https://google.com")

        }
    });
});



builder.Services.AddHttpClient(); //servicio para las consultas a jsonholder


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
