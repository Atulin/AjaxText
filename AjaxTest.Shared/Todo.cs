namespace AjaxTest.Shared;

public sealed record Todo(ShortGuid Id, string Body, DateTimeOffset Time, DateTimeOffset CreatedAt);