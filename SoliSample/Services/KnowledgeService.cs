using OpenAI.Chat;
using System.ClientModel;

namespace SoliSample.Services;

public class KnowledgeService
{  
    private readonly string[] docs;
    private readonly ChatClient _client;  

    public KnowledgeService(IConfiguration _configuration)
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,"docs");

        docs = Directory.Exists(path)
            ? Directory.GetFiles(path, "*.md")
                .Select(File.ReadAllText)
                .ToArray()
            : Array.Empty<string>();

        var apiKey = _configuration["OPENAI_API_KEY"];

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new Exception("OPENAI_API_KEY not found");

        _client = new ChatClient(
            model: "gpt-4.1-mini",
            credential: new ApiKeyCredential(apiKey));
    }

    public async Task<string> Answer(string question)
    {
        if (docs.Length == 0)
            return "No documentation found.";

        var context = Retrieve(question);

        var messages = new ChatMessage[]
        {
            ChatMessage.CreateSystemMessage("""You answer ONLY using provided company documentation.If information is missing say:"I couldn't find that in company docs."""),

            ChatMessage.CreateUserMessage($"""Documentation:{context}Question:{question}""")
        };

        try
        {
            var result =
                await _client.CompleteChatAsync(messages);

            return result.Value.Content[0].Text;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private string Retrieve(string question)
    {
        var words = question
            .ToLower()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var best =
            docs
                .Select(d => new
                {
                    Text = d,
                    Score = words.Count(w =>
                        d.ToLower().Contains(w))
                })
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

        return best?.Text ?? "";
    }
}