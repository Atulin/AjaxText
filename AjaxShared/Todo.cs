namespace AjaxShared;

public sealed record Todo(Guid Id, string Body, DateTimeOffset Time, DateTimeOffset CreatedAt);