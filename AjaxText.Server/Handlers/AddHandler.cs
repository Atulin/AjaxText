using System.Text.Json;
using AjaxTest.Shared;
using AjaxText.Server.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AjaxText.Server.Handlers;

public static class AddHandler
{
	public static async Task<Ok<Todo>> Handle([FromBody]TodoMinimal data)
	{
		var file = await File.ReadAllTextAsync(Constants.FilePath);
	
		var todos = string.IsNullOrWhiteSpace(file) 
			? []
			: JsonSerializer.Deserialize<List<Todo>>(file) ?? [];
	
		var todo = new Todo(new ShortGuid(), data.Body, data.Time, DateTimeOffset.Now);
		todos.Add(todo);

		var json = JsonSerializer.Serialize(todos);
	
		await File.WriteAllTextAsync(Constants.FilePath, json);

		return TypedResults.Ok(todo);
	}
}