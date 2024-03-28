using AjaxText.Server.Handlers;
using AjaxText.Server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

if (!File.Exists(Constants.FilePath)){
	File.Create(Constants.FilePath);
}

app.MapGet("/", () => "Hello World!");

app.MapPost("/add", AddHandler.Handle);
app.MapGet("/all", AllHandler.Handle);
app.MapGet("/find", FindHandler.Handle);

app.Run();
