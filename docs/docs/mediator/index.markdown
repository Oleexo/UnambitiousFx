---
layout: default
title: Mediator
nav_order: 3
permalink: /mediator/
has_children: true
---

# Mediator

UnambitiousFx.Mediator is a lightweight implementation of the mediator pattern for .NET applications. It facilitates loose coupling between components by providing a central communication mechanism.

## Overview

The mediator pattern is a behavioral design pattern that reduces coupling between components by having them communicate indirectly through a mediator object. UnambitiousFx.Mediator implements this pattern with a focus on:

- **Simplicity**: Clean, intuitive API that's easy to understand and use
- **Flexibility**: Support for both request/response and event-based communication
- **Performance**: Optimized for high-performance scenarios, including Native AOT compatibility
- **Extensibility**: Pipeline behaviors for cross-cutting concerns like logging, validation, and error handling

## Key Concepts

### Requests and Handlers

Requests represent operations that should be performed by the application. They can be:

- **Commands**: Requests that change state and may not return a value
- **Queries**: Requests that retrieve data without changing state

Each request has a corresponding handler that contains the logic to process the request.

### Events and Handlers

Events represent notifications that something has happened. They are published by components and can be handled by multiple event handlers.

### Pipeline Behaviors

Pipeline behaviors allow you to add cross-cutting concerns to your request and event processing pipelines, such as logging, validation, and error handling.

## Getting Started

To get started with UnambitiousFx.Mediator, follow these guides:

- [Send a request using mediator](./send-request.html)
- [Publish an event using mediator](./publish-event.html)
- [Creating a RequestPipelineBehavior](./request-pipeline-behavior.html)
- [Creating an EventPipelineBehavior](./event-pipeline-behavior.html)
- [Registering Mediator into dependency injection](./register-mediator.html)
- [Use Mediator.Generator to facilitate the registering into dependency injection](./mediator-generator.html)
