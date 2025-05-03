---
title: Home
layout: home
nav_order: 1
permalink: /
---

# UnambitiousFx

UnambitiousFx is a collection of libraries that facilitate web API development using best practices and a modular approach. It's designed to simplify complex tasks while providing powerful, flexible tools for .NET developers.

{: .fs-6 .fw-300 }

[View on GitHub](https://github.com/oleexo/UnambitiousFx){: .btn .btn-primary .fs-5 .mb-4 .mb-md-0 .mr-2 }

---

## Why Choose UnambitiousFx?

### Simplicity

As Dr. Werner Vogels emphasized, the key to great software lies in hiding complexity while exposing simplicity. UnambitiousFx strives to create intuitive and easy-to-use libraries that abstract away unnecessary complications, focusing on clean, natural interfaces that developers can understand immediately.

### Modular Architecture

UnambitiousFx is a modular ecosystem where each component is like an independent island, connected by clean interfaces and shared principles. Each module is designed to stand on its own while working seamlessly with others. You can choose to use any combination of modules that suits your needs, or easily integrate individual components with your preferred third-party packages.

### Web API Development Utilities

UnambitiousFx is designed to complement ASP.NET Core, not replace it. The libraries provide additional features and utilities that enhance the development of fast, efficient, and maintainable Web APIs while leveraging ASP.NET Core's robust foundation.

### Compatible with NativeAOT

Each library is meticulously designed with Native AOT (Ahead-of-Time) compilation in mind, ensuring optimal performance and efficiency. This results in faster startup times, reduced memory footprint, and improved runtime performance - essential features for modern .NET applications.

---

## Libraries

<div class="grid-container">
  <div class="grid-item">
    <div class="card">
      <div class="card-header">
        <h3>Core</h3>
      </div>
      <div class="card-body">
        <p>A collection of functional programming constructs that enhance error handling and null safety in .NET applications.</p>
        <p>Includes Option, Either, and Result types to help you write more robust and maintainable code.</p>
        <a href="/docs/core/" class="btn btn-outline">Learn More</a>
      </div>
    </div>
  </div>

  <div class="grid-item">
    <div class="card">
      <div class="card-header">
        <h3>Mediator</h3>
      </div>
      <div class="card-body">
        <p>An implementation of the mediator pattern for .NET applications, facilitating loose coupling between components.</p>
        <p>Simplifies request/response handling, command processing, and event distribution in your application.</p>
        <a href="/mediator/" class="btn btn-outline">Learn More</a>
      </div>
    </div>
  </div>

  <div class="grid-item">
    <div class="card">
      <div class="card-header">
        <h3>Messaging</h3>
        <span class="label label-blue">Coming Soon</span>
      </div>
      <div class="card-body">
        <p>A library for handling message-based communication in distributed systems.</p>
        <p>Will provide tools for reliable message processing, queuing, and event-driven architectures.</p>
      </div>
    </div>
  </div>
</div>

<style>
.grid-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 20px;
  margin-top: 2rem;
}

.card {
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 4px;
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.card:hover {
  transform: translateY(-5px);
  box-shadow: 0 10px 20px rgba(0, 0, 0, 0.2);
}

.card-header {
  padding: 1rem;
  background-color: rgba(255, 255, 255, 0.05);
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.card-header h3 {
  margin: 0;
}

.card-body {
  padding: 1rem;
}

.label {
  display: inline-block;
  padding: 0.25rem 0.5rem;
  font-size: 0.75rem;
  font-weight: 500;
  border-radius: 2px;
  margin-left: 0.5rem;
}

.label-blue {
  background-color: #2869e6;
  color: white;
}

.btn-outline {
  display: inline-block;
  padding: 0.375rem 0.75rem;
  border: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 4px;
  background-color: transparent;
  color: inherit;
  text-decoration: none;
  transition: background-color 0.2s ease, border-color 0.2s ease;
}

.btn-outline:hover {
  background-color: rgba(255, 255, 255, 0.1);
  border-color: rgba(255, 255, 255, 0.5);
  text-decoration: none;
}
</style>
