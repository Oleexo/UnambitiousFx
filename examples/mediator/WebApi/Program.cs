using Oleexo.UnambitiousFx.Mediator;
using WebApi.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediator();
builder.Services.AddScoped<ITodoRepository, ITodoRepository>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
