﻿namespace WebApi.Models;

public sealed record CreateTodoModel {
    public required string Name { get; init; }
}
