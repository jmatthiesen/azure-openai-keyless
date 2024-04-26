using Azure.Identity;
using Azure.AI.OpenAI;
using System;
using static System.Environment;

string? endpoint = GetEnvironmentVariable("AZURE_OPENAI_SERVICE");
string? deployment = GetEnvironmentVariable("AZURE_OPENAI_GPT_DEPLOYMENT");

if (endpoint is null || deployment is null) {
    throw new ArgumentNullException("AZURE_OPENAI_SERVICE and AZURE_OPENAI_GPT_DEPLOYMENT environment variables are empty. See README.");
}
OpenAIClient client = new(new Uri(endpoint), new DefaultAzureCredential());

var completionOptions = new ChatCompletionsOptions
{
    DeploymentName=deployment,
    Temperature=0.7f,
    NucleusSamplingFactor=1, // Top P
    Messages =
    {
        new ChatRequestSystemMessage("You are a helpful assistant that makes lots of cat references and uses emojis."),
        new ChatRequestUserMessage("Write a haiku about a hungry cat who wants tuna")
    }
};

ChatCompletions response = await client.GetChatCompletionsAsync(completionOptions);
Console.WriteLine(response.Choices.First().Message.Content);