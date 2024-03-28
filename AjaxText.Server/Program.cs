using System.Text.Json;
using AjaxTest.Shared;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string path = "./todos.json";

if (!File.Exists(path)){
	File.Create(path);
}

app.MapGet("/", () => "Hello World!");

// Add a new todo
app.MapPost("/add", async ([FromBody]TodoMinimal data) => {
	var file = await File.ReadAllTextAsync(path);
	
	var todos = string.IsNullOrWhiteSpace(file) 
		? []
		: JsonSerializer.Deserialize<List<Todo>>(file) ?? [];
	
	var todo = new Todo(Guid.NewGuid(), data.Body, data.Time, DateTimeOffset.Now);
	todos.Add(todo);

	var json = JsonSerializer.Serialize(todos);
	
	await File.WriteAllTextAsync(path, json);

	return Results.Ok(todo);
});

// List all todos
app.MapGet("/all", async () => {
	var file = await File.ReadAllTextAsync(path);

	var todos = string.IsNullOrWhiteSpace(file)
		? []
		: JsonSerializer.Deserialize<List<Todo>>(file) ?? [];

	return todos;
});

// Get a todo by ID
app.MapGet("/find", async (Guid id) => {
	var file = await File.ReadAllTextAsync(path);

	var todos = string.IsNullOrWhiteSpace(file)
		? []
		: JsonSerializer.Deserialize<List<Todo>>(file) ?? [];

	var todo = todos.FirstOrDefault(t => t.Id == id);

	return todo is null ? Results.NotFound() : Results.Ok(todo);
});

app.Run();
