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
    IAsyncEnumerable<string> RunStreaming(string accountId, string modelName,
        IReadOnlyDictionary<string, object?> input, CancellationToken cancellationToken = default);
}
