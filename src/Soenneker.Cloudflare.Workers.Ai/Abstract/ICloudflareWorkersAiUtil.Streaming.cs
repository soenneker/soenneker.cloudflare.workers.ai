using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
using System.Collections.Generic;
using System.Threading;

namespace Soenneker.Cloudflare.Workers.Ai.Abstract;

/// <summary>
/// Defines streaming inference operations for Cloudflare Workers AI models.
/// </summary>
public partial interface ICloudflareWorkersAiUtil
{
    /// <summary>
    /// Runs a model and yields each non-empty server-sent event data payload as it arrives.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="modelName">Name of the model to use.</param>
    /// <param name="input">Model-specific input. The utility adds <c>stream: true</c> to a copy of this dictionary.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>Raw data payloads with the <c>data:</c> prefix removed. The terminal <c>[DONE]</c> event is not returned.</returns>
    [RequiresUnreferencedCode("Arbitrary model inputs require preserved JSON metadata. Use the overload accepting JsonTypeInfo for AOT.")]
    [RequiresDynamicCode("Arbitrary model inputs can require runtime serialization code. Use the overload accepting JsonTypeInfo for AOT.")]
    IAsyncEnumerable<string> RunStreaming(string accountId, string modelName,
        IReadOnlyDictionary<string, object?> input, CancellationToken cancellationToken = default);
    /// <summary>Streams model output using generated JSON metadata for the input dictionary and its runtime values.</summary>
    /// <param name="accountId">Target account.</param>
    /// <param name="modelName">Model identifier.</param>
    /// <param name="input">Model input; stream=true is added to a copy.</param>
    /// <param name="typeInfo">Generated dictionary metadata, including all model-specific runtime value types.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Server-sent data payloads, excluding the terminal DONE event.</returns>
    IAsyncEnumerable<string> RunStreaming(string accountId, string modelName, IReadOnlyDictionary<string, object?> input,
        JsonTypeInfo<Dictionary<string, object?>> typeInfo, CancellationToken cancellationToken = default);

}
