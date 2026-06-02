using SoliSample.Services.Interfaces;

namespace SoliSample.Services;

public sealed class KnowledgeService : IKnowledgeService
{
    private readonly IRetrievalService _retrieval;
    private readonly ILLMService _llm;

    public KnowledgeService(
        IRetrievalService retrieval,
        ILLMService llm)
    {
        _retrieval = retrieval;
        _llm = llm;
    }

    public async Task<string> AnswerAsync(string question)
    {
        var context =
            _retrieval.Retrieve(question);

        return await _llm.GenerateAnswerAsync(
            context,
            question);
    }
}