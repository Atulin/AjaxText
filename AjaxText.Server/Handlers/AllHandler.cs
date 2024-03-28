using System.Text.Json;
using AjaxTest.Shared;
using AjaxText.Server.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AjaxText.Server.Handlers;

public static class AllHandler
{
	public static async Task<Ok<List<Todo>>> Handle()
	{
		var file = await File.ReadAllTextAsync(Constants.FilePath);

		var todos = string.IsNullOrWhiteSpace(file)
			? []
			: JsonSerializer.Deserialize<List<Todo>>(file) ?? [];

		return TypedResults.Ok(todos);
	}
}