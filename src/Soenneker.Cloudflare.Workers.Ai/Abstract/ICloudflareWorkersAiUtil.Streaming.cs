using System.Collections.Generic;
using System.Threading;

namespace Soenneker.Cloudflare.Workers.Ai.Abstract;

public partial interface ICloudflareWorkersAiUtil
{
    /// <summary>Runs a model and yields each server-sent event data payload as it arrives.</summary>
    IAsyncEnumerable<string> RunStreaming(string accountId, string modelName,
        IReadOnlyDictionary<string, object?> input, CancellationToken cancellationToken = default);
}
