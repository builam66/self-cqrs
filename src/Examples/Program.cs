using Carter;
using CQRS;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();

// Add CQRS
builder.Services.AddCQRS(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MapCarter();

app.Run();
