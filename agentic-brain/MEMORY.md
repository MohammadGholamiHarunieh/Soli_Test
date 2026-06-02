# Memory

Initial implementation mixed:

- retrieval
- prompt generation
- HTTP communication

inside one class.

Refactored to respect SOLID principles.

Lessons learned:

- Dependency inversion improves testing.
- Retrieval should be independent from LLM.
- Configuration should live in appsettings.