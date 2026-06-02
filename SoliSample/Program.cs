using SoliSample.Options;
using SoliSample.Services;
using SoliSample.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<OpenRouterOptions>(
    builder.Configuration.GetSection("OpenRouter"));

builder.Services.AddSingleton<IRetrievalService,
    FileRetrievalService>();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<ILLMService, OpenRouterService>();

builder.Services.AddSingleton<IKnowledgeService,
    KnowledgeService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
