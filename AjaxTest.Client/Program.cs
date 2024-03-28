using System.Net.Http.Json;
using System.Web;
using AjaxTest.Shared;
using Spectre.Console;
using Spectre.Console.Rendering;

AnsiConsole.WriteLine("Hello, World!");

var client = new HttpClient();
client.BaseAddress = new Uri("https://localhost:7082");

string? cmd;
do {
	cmd = AnsiConsole.Prompt(new SelectionPrompt<string>()
		.Title("[bold]What do you want to do?[/]")
		.PageSize(10)
		.AddChoices(["List all", "Add new", "Find", "Quit"]));

	try{
		switch (cmd){
			case "List all":
				await All();
				break;
			case "Add new":
				await Add();
				break;
			case "Find":
				await Find();
				break;
			case "Quit":
                AnsiConsole.MarkupLine("[red]Closing...[/]");
				break;
			default:
                AnsiConsole.MarkupLine("[red]Unknown command[/]");
				break;
		}
	}
	catch (Exception e){
        AnsiConsole.MarkupLineInterpolated($"[red]An error has occurred: [bold]{e.Message}[/][/]");
	}

} while (cmd is not null and not "Quit");
return;

async Task All()
{
	var todos = await client.GetFromJsonAsync<List<Todo>>("all");
	foreach (var todo in todos ?? []){
		var panel = new Panel($"""
		    Due [bold]{todo.Time}[/]
		    [italic green]{todo.Body}[/]
		    """).Header(todo.Id);
		AnsiConsole.Write(panel);
	}
}

async Task Add()
{
	var body = AnsiConsole.Ask<string>("[bold]Enter body:[/]");
	var due = AnsiConsole.Ask<DateTimeOffset>("[bold]Enter due date:[/]");

	var todo = new TodoMinimal(body, due);
	var res = await client.PostAsJsonAsync("add", todo);

	var data = await res.Content.ReadFromJsonAsync<Todo>();

	var msg = res.IsSuccessStatusCode && data is not null
		? $"[green]Created todo with id {data.Id}[/]" 
		: $"[red]Error {res.StatusCode} {res.ReasonPhrase}[/]";

    AnsiConsole.MarkupLine(msg);
}

async Task Find()
{
	var id = AnsiConsole.Ask<ShortGuid>("[bold]Enter ID:[/]");

	var todo = await client.GetFromJsonAsync<Todo>($"find?id={HttpUtility.UrlEncode(id)}");

	Renderable msg = todo is null
		? new Markup("[red]Todo not found[/]") 
		: new Panel($"""
              [gray]Created [bold]{todo.CreatedAt}[/][/]
              Due [bold]{todo.Time}[/]
              [italic green]{todo.Body}[/]
              """).Header(todo.Id);

    AnsiConsole.Write(msg);
}