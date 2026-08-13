using Soenneker.Cloudflare.OpenApiClient.Models;
using Soenneker.Cloudflare.Workers.Ai.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Workers.Ai.Abstract;

public partial interface ICloudflareWorkersAiUtil
{
    /// <summary>Runs a model through Cloudflare's generic Workers AI endpoint.</summary>
    ValueTask<WorkersAiPostRunGeneric200?> RunGeneric(string accountId, WorkersAiPostRunGeneric request,
        CancellationToken cancellationToken = default);

    /// <summary>Generates text from a prompt.</summary>
    ValueTask<WorkersAiPostRunGeneric200?> GenerateText(string accountId, string modelName, string prompt,
        int? maxTokens = null, double? temperature = null, CancellationToken cancellationToken = default);

    /// <summary>Runs a conversational model with a sequence of messages.</summary>
    ValueTask<WorkersAiPostRunGeneric200?> Chat(string accountId, string modelName,
        IReadOnlyCollection<CloudflareWorkersAiMessage> messages, int? maxTokens = null, double? temperature = null,
        CancellationToken cancellationToken = default);

    /// <summary>Generates vector embeddings for one or more strings.</summary>
    ValueTask<WorkersAiPostRunGeneric200?> GenerateEmbeddings(string accountId, string modelName,
        IReadOnlyCollection<string> text, CancellationToken cancellationToken = default);

    /// <summary>Generates an image from a prompt.</summary>
    ValueTask<WorkersAiPostRunGeneric200?> GenerateImage(string accountId, string modelName, string prompt,
        string? negativePrompt = null, int? width = null, int? height = null, int? steps = null,
        CancellationToken cancellationToken = default);

    /// <summary>Transcribes an audio payload.</summary>
    ValueTask<WorkersAiPostRunGeneric200?> Transcribe(string accountId, string modelName, byte[] audio,
        string? language = null, CancellationToken cancellationToken = default);

    /// <summary>Translates text between languages.</summary>
    ValueTask<WorkersAiPostRunGeneric200?> Translate(string accountId, string modelName, string text,
        string sourceLanguage, string targetLanguage, CancellationToken cancellationToken = default);
}
