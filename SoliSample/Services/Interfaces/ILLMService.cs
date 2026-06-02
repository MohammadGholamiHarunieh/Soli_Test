namespace SoliSample.Services.Interfaces;

public interface ILLMService
{
    Task<string> GenerateAnswerAsync(
        string context,
        string question);
}