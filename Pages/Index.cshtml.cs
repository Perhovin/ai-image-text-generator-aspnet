namespace AiDemo.Pages;

using System.Text;
using System.Text.Json;
using AiDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    [BindProperty]
    public string Prompt { get; set; } = string.Empty;

    [BindProperty]
    public string TextPrompt { get; set; } = string.Empty;

    public string GeneratedImageUrl { get; set; } = "https://placehold.co/800x500?text=Image+Preview";

    public string TextAnswer { get; set; } = string.Empty;

    public IndexModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task OnPostGenerateImage()
    {
        var endpoint = _configuration["Foundry:Endpoint"];
        var apiKey = _configuration["Foundry:ApiKey"];
        var model = _configuration["Foundry:ImageModel"];

        if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(model))
        {
            GeneratedImageUrl = "https://placehold.co/800x500?text=Missing+secrets";
            return;
        }

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("api-key", apiKey);

        var request = new ImageGenerationRequest
        {
            Prompt = Prompt,
            Width = 1024,
            Height = 768,
            Model = model
        };

        var requestJson = JsonSerializer.Serialize(request);
        using var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(
            $"{endpoint.TrimEnd('/')}/mai/v1/images/generations",
            content);

        if (response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();

            using JsonDocument json = JsonDocument.Parse(body);
            string? base64Image = json.RootElement
                .GetProperty("data")[0]
                .GetProperty("b64_json")
                .GetString();

            GeneratedImageUrl = !string.IsNullOrWhiteSpace(base64Image)
                ? $"data:image/png;base64,{base64Image}"
                : "https://placehold.co/800x500?text=Image+Result+Missing";
        }
        else
        {
            GeneratedImageUrl = "https://placehold.co/800x500?text=Image+Request+Failed";
        }
    }

    public async Task OnPostGenerateText()
    {
        var endpoint = _configuration["Foundry:Endpoint"];
        var apiKey = _configuration["Foundry:ApiKey"];
        var model = _configuration["Foundry:TextModel"];

        if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(model))
        {
            TextAnswer = "Missing GPT-5 secret configuration.";
            return;
        }

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("api-key", apiKey);

        var request = new TextGenerationRequest
        {
            Model = model,
            Messages = { new ChatMessage { Role = "user", Content = TextPrompt } }
        };

        var requestJson = JsonSerializer.Serialize(request);
        using var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(
            $"{endpoint.TrimEnd('/')}/models/chat/completions?api-version=2024-05-01-preview",
            content);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TextGenerationResponse>();
            TextAnswer = result?.Answer ?? "No answer was returned.";
        }
        else
        {
            TextAnswer = $"The GPT-5 request failed: {(int)response.StatusCode} {await response.Content.ReadAsStringAsync()}";
        }
    }
}
