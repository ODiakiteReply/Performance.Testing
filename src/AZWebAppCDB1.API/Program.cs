using AutoMapper;
using AZWebAppCDB1.Translators;
using System.Text.Json.Serialization;
using AZWebAppCDB1.Business.Extensions;

var builder = WebApplication.CreateBuilder(args);

// AppSettings iniettato tramite chart da Kubernetes al pod
builder.Configuration.AddJsonFile("/app/settings/appsettings.json", true);

// Service Principal iniettato tramite chart da Kubernetes al pod e utilizzato per l'accesso al DB
builder.Configuration.AddKeyPerFile("/app/secrets-sp-secret", true);

// Add services to the container.
builder.Services.AddBusinessServices();

builder.Services.AddControllers().AddJsonOptions(o => {
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new Translator());
    cfg.ConstructServicesUsing(provider.GetService);
}).CreateMapper());

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Any", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

//app.MigrateDataBase(builder.Configuration);

app.UseCors("Any");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
