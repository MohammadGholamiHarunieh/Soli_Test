# Mini Company Knowledge Bot

A lightweight AI-assisted knowledge bot that answers questions using internal company documentation.

The project demonstrates:

* Clean Architecture principles
* SOLID design
* Separation of Retrieval and Answer Generation
* AI-assisted development workflow
* Context and memory management for agentic coding

---

## Overview

This application loads internal company documents from the `docs` directory and allows users to ask questions through a REST API.

The system retrieves the most relevant document and uses an LLM (via OpenRouter) to generate an answer strictly based on the provided documentation.

Example use cases:

* Employee FAQ
* Leave policy questions
* Product information lookup
* Support documentation

---

## Features

* REST API endpoint for asking questions
* Markdown-based knowledge source
* Retrieval layer separated from LLM layer
* OpenRouter integration
* Dependency Injection
* Configurable LLM provider
* Agentic Brain documentation

---

## Architecture

```text
Client
   |
   v
Controller
   |
   v
IKnowledgeService
   |
   +------------------+
   |                  |
   v                  v
IDocumentRepository   ILLMService
   |                  |
   v                  v
Markdown Files     OpenRouter
```

### Responsibilities

#### KnowledgeService

Coordinates the complete workflow:

1. Receive user question
2. Retrieve relevant content
3. Request answer from LLM
4. Return response

#### Document Repository

Responsible for:

* Loading documents
* Searching documents
* Returning relevant content

#### LLM Service

Responsible for:

* Prompt creation
* OpenRouter communication
* Response parsing

---

## Project Structure

```text
SoliSample/

├── Controllers/
│   └── HomeController.cs
│
├── Models/
│   └── AskRequest.cs
│
├── Services/
│   ├── Interfaces/
│   │   ├── IKnowledgeService.cs
│   │   ├── ILLMService.cs
│   │   └── IDocumentRepository.cs
│   │
│   ├── KnowledgeService.cs
│   ├── OpenRouterService.cs
│   └── DocumentRepository.cs
│
├── Options/
│   └── OpenRouterOptions.cs
│
├── docs/
│   ├── faq.md
│   ├── leave-policy.md
│   └── product.md
│
├── agentic-brain/
│   ├── PROJECT_BRIEF.md
│   ├── AGENT_CONTEXT.md
│   ├── MEMORY.md
│   ├── TASKS.md
│   └── EVALS.md
│
├── Program.cs
└── appsettings.json
```

---

## API

### Ask Question

**POST**

```http
/api/ask
```

Request:

```json
{
  "question": "How many leave days do employees receive?"
}
```

Response:

```json
{
  "answer": "Employees receive 20 paid leave days per year."
}
```

---

## Configuration

Update `appsettings.json`:

```json
{
  "OpenRouter": {
    "ApiKey": "YOUR_API_KEY",
    "Model": "deepseek/deepseek-r1:free",
    "BaseUrl": "https://openrouter.ai/api/v1"
  }
}
```

---

## Running the Project

### Prerequisites

* .NET 8 SDK
* OpenRouter API Key

### Run

```bash
dotnet restore
dotnet build
dotnet run
```

Swagger:

```text
https://localhost:{port}/swagger
```

---

## Example Knowledge Sources

The bot currently uses three sample documents:

* FAQ
* Leave Policy
* Product Information

Additional markdown files can be added to the `docs` directory without changing the application code.

---

## Design Decisions

### Why separate retrieval from LLM?

Keeping retrieval independent provides:

* Better maintainability
* Easier testing
* Ability to replace retrieval strategy
* Ability to add vector search later

### Why interfaces?

Interfaces allow:

* Dependency inversion
* Mocking during tests
* Easy replacement of implementations

### Why OpenRouter?

OpenRouter provides:

* Access to multiple LLM providers
* Model flexibility
* Lower vendor lock-in

---

## Future Improvements

* Vector database integration
* Embeddings-based retrieval
* Semantic search
* Document chunking
* Source citations
* Conversation memory
* Unit tests
* Integration tests
* Docker support

---

## AI Usage Report

### Tools Used

* ChatGPT
* GitHub Copilot

### AI Assisted Tasks

* Initial project scaffolding
* Architectural brainstorming
* Refactoring suggestions
* Prompt design
* Documentation generation

### Human Decisions

* Final architecture selection
* Service boundaries
* Dependency injection strategy
* Retrieval approach
* API design

### AI Outputs Rejected or Modified

* Overly coupled service implementations
* Direct LLM calls inside controllers
* Hardcoded configuration values

Several AI-generated suggestions were manually reviewed and refactored to better align with SOLID principles and maintainability goals.

---

## Evaluation Questions

1. How many leave days do employees receive?
2. What are the support hours?
3. What is the company product?
4. Do unused leave days expire?
5. What support is available for priority customers?

Expected answers are documented in:

```text
agentic-brain/EVALS.md
```

---

## License

This project was created as part of an AI Builder sample and is intended for educational and evaluation purposes.
