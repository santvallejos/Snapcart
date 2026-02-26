using Azure;
using Azure.AI.Inference;
using Snapcart.Application.Interfaces;

namespace Snapcart.Application.Services;

public class DetectProductService(string token) : IDetectProductService
{
    private readonly ChatCompletionsClient _client = new(
        new Uri("https://models.inference.ai.azure.com"),
        new AzureKeyCredential(token));

    public async Task<string> DetectProductAsync(Stream imageStream, string fileName)
    {
        using var ms = new MemoryStream();
        await imageStream.CopyToAsync(ms);
        var imageBytes = ms.ToArray();
        var base64 = Convert.ToBase64String(imageBytes);

        var extension = Path.GetExtension(fileName)?.TrimStart('.').ToLowerInvariant() ?? "jpeg";
        var mimeType = extension switch
        {
            "png" => "png",
            "gif" => "gif",
            "webp" => "webp",
            _ => "jpeg"
        };
        var dataUrl = $"data:image/{mimeType};base64,{base64}";

        var requestOptions = new ChatCompletionsOptions
        {
            Messages =
            {
                new ChatRequestSystemMessage(
                    "You are a product detection assistant. Analyze the image and identify the product. " +
                    "Respond only with the product name, nothing else."),
                new ChatRequestUserMessage(
                [
                    new ChatMessageTextContentItem("What product is in this image?"),
                    new ChatMessageImageContentItem(new Uri(dataUrl))
                ])
            },
            MaxTokens = 256,
            Model = "gpt-4o"
        };

        var response = await _client.CompleteAsync(requestOptions);
        return response.Value.Content;
    }
}
