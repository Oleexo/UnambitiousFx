---
layout: default
title: Basics
parent: Mediator
nav_order: 10
has_children: true
---

# Basics

The essentials to get productive with UnambitiousFx.Mediator. This section focuses on the core building blocks you use every day:

- Define and send requests (with or without a response)
- Publish events and react with event handlers
- Register mediator, handlers, and behaviors in DI

All examples are AOT‑friendly and rely on standard .NET DI — no runtime code‑gen.

## What you’ll learn
- When to model an operation as a request vs. an event
- How to define a request and its handler returning `Result`/`Result<T>`
- How to publish domain events and handle them
- How to register the mediator and your components with `AddMediator(...)`

If you prefer a 30‑second tour, see the Mediator landing page’s Quick start: ../index.html

## Quick links
- Send a request: ./send-request.html — Define `IRequest<T>`/`IRequest` and call `ISender.SendAsync(...)`.
- Publish an event: ./publish-event.html — Define `IEvent`, publish, and handle with one or many `IEventHandler<TEvent>`.
- Register mediator (DI): ./register-mediator.html — Add mediator, handlers, and pipeline behaviors via `AddMediator`.

## See also
- Behaviors (cross‑cutting concerns): ../behaviors/index.html
- Glossary: ../glossary.html