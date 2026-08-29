using Soenneker.Cloudflare.OpenApiClient.Models;
using Soenneker.Cloudflare.Workers.Ai.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Workers.Ai.Abstract;

public partial interface ICloudflareWorkersAiUtil
{
    /// <summary>
    /// Runs a model through Cloudflare's generic Workers AI endpoint.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="request">request that defines the request to send.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Post Run Generic.</returns>
    ValueTask<WorkersAiPostRunGeneric200?> RunGeneric(string accountId, WorkersAiPostRunGeneric request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates text from a prompt.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="modelName">Name of the model to use.</param>
    /// <param name="prompt">Prompt for the generate text operation.</param>
    /// <param name="maxTokens">Max Tokens for the generate text operation.</param>
    /// <param name="temperature">Temperature for the generate text operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Post Run Generic.</returns>
    ValueTask<WorkersAiPostRunGeneric200?> GenerateText(string accountId, string modelName, string prompt,
        int? maxTokens = null, double? temperature = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Runs a conversational model with a sequence of messages.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="modelName">Name of the model to use.</param>
    /// <param name="messages">Messages to send or process.</param>
    /// <param name="maxTokens">Max Tokens for the chat operation.</param>
    /// <param name="temperature">Temperature for the chat operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Post Run Generic.</returns>
    ValueTask<WorkersAiPostRunGeneric200?> Chat(string accountId, string modelName,
        IReadOnlyCollection<CloudflareWorkersAiMessage> messages, int? maxTokens = null, double? temperature = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates vector embeddings for one or more strings.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="modelName">Name of the model to use.</param>
    /// <param name="text">text to process.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Post Run Generic.</returns>
    ValueTask<WorkersAiPostRunGeneric200?> GenerateEmbeddings(string accountId, string modelName,
        IReadOnlyCollection<string> text, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates an image from a prompt.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="modelName">Name of the model to use.</param>
    /// <param name="prompt">Prompt for the generate image operation.</param>
    /// <param name="negativePrompt">Negative Prompt for the generate image operation.</param>
    /// <param name="width">Width to apply.</param>
    /// <param name="height">Height to apply.</param>
    /// <param name="steps">Steps for the generate image operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Post Run Generic.</returns>
    ValueTask<WorkersAiPostRunGeneric200?> GenerateImage(string accountId, string modelName, string prompt,
        string? negativePrompt = null, int? width = null, int? height = null, int? steps = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Transcribes an audio payload.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="modelName">Name of the model to use.</param>
    /// <param name="audio">audio to process.</param>
    /// <param name="language">Language for the transcribe operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Post Run Generic.</returns>
    ValueTask<WorkersAiPostRunGeneric200?> Transcribe(string accountId, string modelName, byte[] audio,
        string? language = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Translates text between languages.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="modelName">Name of the model to use.</param>
    /// <param name="text">Text to read, write, or transform.</param>
    /// <param name="sourceLanguage">source Language to read or transform.</param>
    /// <param name="targetLanguage">Target Language for the translate operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Post Run Generic.</returns>
    ValueTask<WorkersAiPostRunGeneric200?> Translate(string accountId, string modelName, string text,
        string sourceLanguage, string targetLanguage, CancellationToken cancellationToken = default);
}
