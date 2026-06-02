using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SoliSample.Options;
using SoliSample.Services.Interfaces;

namespace SoliSample.Services;

public sealed class OpenRouterService : ILLMService
{
    private readonly HttpClient _httpClient;
    private readonly OpenRouterOptions _options;

    public OpenRouterService(
        IHttpClientFactory httpClientFactory,
        IOptions<OpenRouterOptions> options)
    {
        _httpClient = httpClientFactory.CreateClient();
        _options = options.Value;

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                _options.ApiKey);

        _httpClient.DefaultRequestHeaders.Add(
            "HTTP-Referer",
            "http://localhost");

        _httpClient.DefaultRequestHeaders.Add(
            "X-Title",
            "MiniCompanyKnowledgeBot");
    }

    public async Task<string> GenerateAnswerAsync(
        string context,
        string question)
    {
        var payload = new
        {
            model = _options.Model,
            max_tokens = 200,
            temperature = 0.1,
            messages = new object[]
            {
                new
                {
                    role = "system",
                    content =
                        """
                        You answer ONLY from the provided company documentation.

                        If the answer is not present in the documentation,
                        say:
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

        var response =
            await _httpClient.PostAsJsonAsync(
                $"{_options.BaseUrl}/chat/completions",
                payload);

        var responseBody =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return $"OpenRouter Error: {response.StatusCode}\n{responseBody}";
        }

        using var json =
            JsonDocument.Parse(responseBody);

        return json
            .RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString()
            ?? "No answer generated.";
    }
}