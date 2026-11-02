namespace AiDemo.Models;

using System.Text.Json.Serialization;

public class TextGenerationResponse
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("choices")]
    public List<Choice>? Choices { get; set; }

    public string? Answer => Choices?.FirstOrDefault()?.Message?.Content;
}

public class Choice
{
    [JsonPropertyName("message")]
    public ChatMessage? Message { get; set; }
}
