using System.Text.Json;
using AjaxTest.Shared;
using AjaxText.Server.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AjaxText.Server.Handlers;

public static class FindHandler
{
	public static async Task<Results<Ok<Todo>, NotFound>> Handle([FromQuery]ShortGuid id)
	{
		var file = await File.ReadAllTextAsync(Constants.FilePath);

		var todos = string.IsNullOrWhiteSpace(file)
			? []
			: JsonSerializer.Deserialize<List<Todo>>(file) ?? [];

		var todo = todos.FirstOrDefault(t => t.Id == id);

		return todo is null ? TypedResults.NotFound() : TypedResults.Ok(todo);
	}
}