namespace AjaxTest.Shared;

public sealed record Todo(Guid Id, string Body, DateTimeOffset Time, DateTimeOffset CreatedAt);