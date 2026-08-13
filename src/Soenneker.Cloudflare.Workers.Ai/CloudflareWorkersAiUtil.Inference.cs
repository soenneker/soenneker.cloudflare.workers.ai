using Microsoft.Extensions.Logging;
using Soenneker.Cloudflare.OpenApiClient;
using Soenneker.Cloudflare.OpenApiClient.Models;
using Soenneker.Cloudflare.Workers.Ai.Dtos;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Workers.Ai;

public sealed partial class CloudflareWorkersAiUtil
{
    public async ValueTask<WorkersAiPostRunGeneric200?> RunGeneric(string accountId, WorkersAiPostRunGeneric request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accountId);
        ArgumentNullException.ThrowIfNull(request);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Model);

        _logger.LogInformation("Running Workers AI model {ModelName} through the generic endpoint in account {AccountId}", request.Model, accountId);

        try
        {
            CloudflareOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();
            WorkersAiPostRunGeneric200? response = await client.Accounts[accountId].Ai.Run
                .PostAsync(request, cancellationToken: cancellationToken)
                .NoSync();

            _logger.LogInformation("Successfully ran Workers AI model {ModelName} through the generic endpoint", request.Model);
            return response;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to run Workers AI model {ModelName} through the generic endpoint in account {AccountId}",
                request.Model, accountId);
            throw;
        }
    }

    public ValueTask<WorkersAiPostRunGeneric200?> GenerateText(string accountId, string modelName, string prompt, int? maxTokens = null,
        double? temperature = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt);

        var input = new Dictionary<string, object> { ["prompt"] = prompt };
        AddOptional(input, "max_tokens", maxTokens);
        AddOptional(input, "temperature", temperature);
        return RunGeneric(accountId, CreateGenericRequest(modelName, input), cancellationToken);
    }

    public ValueTask<WorkersAiPostRunGeneric200?> Chat(string accountId, string modelName, IReadOnlyCollection<CloudflareWorkersAiMessage> messages,
        int? maxTokens = null, double? temperature = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(messages);
        if (messages.Count == 0)
            throw new ArgumentException("At least one message must be supplied.", nameof(messages));

        List<Dictionary<string, object>> payload = messages.Select(message =>
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(message.Role);
            ArgumentException.ThrowIfNullOrWhiteSpace(message.Content);
            return new Dictionary<string, object> { ["role"] = message.Role, ["content"] = message.Content };
        }).ToList();

        var input = new Dictionary<string, object> { ["messages"] = payload };
        AddOptional(input, "max_tokens", maxTokens);
        AddOptional(input, "temperature", temperature);
        return RunGeneric(accountId, CreateGenericRequest(modelName, input), cancellationToken);
    }

    public ValueTask<WorkersAiPostRunGeneric200?> GenerateEmbeddings(string accountId, string modelName, IReadOnlyCollection<string> text,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(text);
        if (text.Count == 0)
            throw new ArgumentException("At least one string must be supplied.", nameof(text));

        return RunGeneric(accountId, CreateGenericRequest(modelName, new Dictionary<string, object> { ["text"] = text.ToArray() }), cancellationToken);
    }

    public ValueTask<WorkersAiPostRunGeneric200?> GenerateImage(string accountId, string modelName, string prompt, string? negativePrompt = null,
        int? width = null, int? height = null, int? steps = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prompt);

        var input = new Dictionary<string, object> { ["prompt"] = prompt };
        AddOptional(input, "negative_prompt", negativePrompt);
        AddOptional(input, "width", width);
        AddOptional(input, "height", height);
        AddOptional(input, "num_steps", steps);
        return RunGeneric(accountId, CreateGenericRequest(modelName, input), cancellationToken);
    }

    public ValueTask<WorkersAiPostRunGeneric200?> Transcribe(string accountId, string modelName, byte[] audio, string? language = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(audio);
        if (audio.Length == 0)
            throw new ArgumentException("Audio content cannot be empty.", nameof(audio));

        var input = new Dictionary<string, object> { ["audio"] = Convert.ToBase64String(audio) };
        AddOptional(input, "language", language);
        return RunGeneric(accountId, CreateGenericRequest(modelName, input), cancellationToken);
    }

    public ValueTask<WorkersAiPostRunGeneric200?> Translate(string accountId, string modelName, string text, string sourceLanguage,
        string targetLanguage, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceLanguage);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetLanguage);

        var input = new Dictionary<string, object>
        {
            ["text"] = text,
            ["source_lang"] = sourceLanguage,
            ["target_lang"] = targetLanguage
        };
        return RunGeneric(accountId, CreateGenericRequest(modelName, input), cancellationToken);
    }

    private static WorkersAiPostRunGeneric CreateGenericRequest(string modelName, Dictionary<string, object> input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(modelName);
        return new WorkersAiPostRunGeneric
        {
            Model = modelName,
            Input = new WorkersAiPostRunGeneric_input { AdditionalData = input }
        };
    }

    private static void AddOptional<T>(Dictionary<string, object> input, string key, T? value)
    {
        if (value is not null)
            input[key] = value;
    }
}
