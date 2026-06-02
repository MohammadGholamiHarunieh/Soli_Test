namespace SoliSample.Services.Interfaces;

public interface IKnowledgeService
{
    Task<string> AnswerAsync(string question);
}