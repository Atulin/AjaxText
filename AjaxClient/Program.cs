using System.Globalization;
using System.Net.Http.Json;
using AjaxShared;

Console.WriteLine("Hello, World!");

var client = new HttpClient();
client.BaseAddress = new Uri("https://localhost:7082");

string? cmd;
do {
	Console.WriteLine("""
	    [q]    Exit
	    [all]  List all todos
	    [add]  Add new todo
	    [find] Find todo by ID              
	    """);
	cmd = Console.ReadLine();

	try{
		switch (cmd){
			case "all":
				await All();
				break;
			case "add":
				await Add();
				break;
			case "find":
				await Find();
				break;
			case "q":
				Console.WriteLine("Closing...");
				break;
			default:
				Console.WriteLine("Unknown command");
				break;
		}
	}
	catch (Exception e){
		Console.WriteLine($"An error has occurred: {e.Message}");
	}

} while (cmd is not null and not "q");
return;

async Task All()
{
	var todos = await client.GetFromJsonAsync<List<Todo>>("all");
	foreach (var todo in todos ?? []){
		Console.WriteLine($"""
	       {todo.Id}
	       Created at {todo.CreatedAt}
	       Due by {todo.Time}
	           {todo.Body}
	       ---
	       """);
	}
}

async Task Add()
{
	Console.WriteLine("Enter body:");
	var body = Console.ReadLine() ?? "";

	const string format = "yyyy-MM-dd HH:mm";
	Console.WriteLine($"Enter due date ({format}):");
	DateTimeOffset due;
	while (!DateTimeOffset.TryParseExact(Console.ReadLine(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out due)){
		Console.WriteLine("Incorrect format!");
	}

	var todo = new TodoMinimal(body, due);
	var res = await client.PostAsJsonAsync("add", todo);

	var data = await res.Content.ReadFromJsonAsync<Todo>();

	var msg = res.IsSuccessStatusCode && data is not null
		? $"Created todo with id {data.Id}" 
		: $"Error {res.StatusCode} {res.ReasonPhrase}";
	
	Console.WriteLine(msg);
}

async Task Find()
{
	Console.WriteLine("Enter ID:");
	Guid id;
	while (!Guid.TryParse(Console.ReadLine(), out id)){
		Console.WriteLine("Incorrect format!");
	}

	var todo = await client.GetFromJsonAsync<Todo>($"find?id={id}");
	
	var msg = todo is null
		? "Todo not found" 
		:  $"""
	       {todo.Id}
	       Created at {todo.CreatedAt}
	       Due by {todo.Time}
	           {todo.Body}
	       """;
	
	Console.WriteLine(msg);
}