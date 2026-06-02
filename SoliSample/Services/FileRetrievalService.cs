using SoliSample.Services.Interfaces;

namespace SoliSample.Services;

public sealed class FileRetrievalService
    : IRetrievalService
{
    private readonly string[] _docs;

    public FileRetrievalService()
    {
        var path =
            Path.Combine(
                AppContext.BaseDirectory,
                "docs");

        _docs = Directory.Exists(path)
            ? Directory.GetFiles(path, "*.md")
                .Select(File.ReadAllText)
                .ToArray()
            : Array.Empty<string>();
    }

    public string Retrieve(string question)
    {
        var words = question
            .ToLowerInvariant()
            .Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

        var best =
            _docs
                .Select(d => new
                {
                    Text = d,
                    Score = words.Count(
                        w => d.Contains(
                            w,
                            StringComparison.OrdinalIgnoreCase))
                })
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

        return best?.Text ?? "";
    }
}