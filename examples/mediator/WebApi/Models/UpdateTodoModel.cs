﻿namespace WebApi.Models;

public sealed record UpdateTodoModel {
    public required string Name { get; init; }
}
