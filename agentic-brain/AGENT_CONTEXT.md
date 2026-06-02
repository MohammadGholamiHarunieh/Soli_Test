# Agent Context

Architecture:

Controller
    -> KnowledgeService
    -> RetrievalService
    -> LLMService

Retrieval and answer generation are intentionally separated.

To add another LLM provider:

1. Implement ILLMService
2. Register in DI
3. No changes required elsewhere