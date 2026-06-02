using System.Net.Http.Headers;
using System.Text.Json;

namespace SoliSample.Services;

public class KnowledgeService
{
    private readonly string[] docs;
    private readonly HttpClient _httpClient;
    private readonly string _modelName;

    public KnowledgeService(IConfiguration configuration)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "docs");

        docs = Directory.Exists(path)
            ? Directory.GetFiles(path, "*.md")
                .Select(File.ReadAllText)
                .ToArray()
            : Array.Empty<string>();

        var apiKey = configuration["OPENAI_API_KEY"];

        _modelName = configuration["OPENAI_MODEL"]
                     ?? "openai/gpt-oss-120b:free";

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new Exception("OPENAI_API_KEY not found");

        _httpClient = new HttpClient();

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);

        _httpClient.DefaultRequestHeaders.Add(
            "HTTP-Referer",
            "http://localhost");

        _httpClient.DefaultRequestHeaders.Add(
            "X-Title",
            "MiniCompanyKnowledgeBot");
    }

    public async Task<string> Answer(string question)
    {
        if (docs.Length == 0)
            return "No documentation found.";

        var context = Retrieve(question);
        
        var payload = new
        {
            model = _modelName,
            max_tokens = 200,
            temperature = 0.1,
            messages = new object[]
            {
                new
                {
                    role = "system",                    
                    content =
                        """
                        You are a company knowledge bot.

                        Answer ONLY from the provided documentation.

                        If the answer is not found in the documentation,
                        respond with:
                        "I couldn't find that in company docs."
                        """
                },
                new
                {
                    role = "user",
                    content =
                        $"""
                        Documentation:

                        {context}

                        Question:
                        {question}
                        """
                }
            }
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "https://openrouter.ai/api/v1/chat/completions",
                payload);

            var responseBody =
                await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"OpenRouter Error: {response.StatusCode}\n{responseBody}";
            }

            using var document =
                JsonDocument.Parse(responseBody);

            return document
                       .RootElement
                       .GetProperty("choices")[0]
                       .GetProperty("message")
                       .GetProperty("content")
                       .GetString()
                   ?? "No response returned.";
        }
        catch (Exception ex)
        {
            return $"Exception: {ex.Message}";
        }
    }

    private string Retrieve(string question)
    {
        var words = question
            .ToLowerInvariant()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var bestMatch = docs
            .Select(d => new
            {
                Text = d,
                Score = words.Count(w =>
                    d.Contains(w, StringComparison.OrdinalIgnoreCase))
            })
            .OrderByDescending(x => x.Score)
            .FirstOrDefault();

        return bestMatch?.Text ?? string.Empty;
    }
}