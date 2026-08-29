using System.Collections.Generic;
using System.Threading;

namespace Soenneker.Cloudflare.Workers.Ai.Abstract;

/// <summary>
/// Defines streaming inference operations for Cloudflare Workers AI models.
/// </summary>
public partial interface ICloudflareWorkersAiUtil
{
    /// <summary>
    /// Runs a model and yields each server-sent event data payload as it arrives.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="modelName">Name of the model to use.</param>
    /// <param name="input">input to read or transform.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The resulting async Enumerable.</returns>
    IAsyncEnumerable<string> RunStreaming(string accountId, string modelName,
        IReadOnlyDictionary<string, object?> input, CancellationToken cancellationToken = default);
}
