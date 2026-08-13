using Soenneker.Cloudflare.OpenApiClient.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Workers.Ai.Abstract;

public partial interface ICloudflareWorkersAiUtil
{
    /// <summary>Lists models in the Workers AI catalog.</summary>
    ValueTask<WorkersAiSearchModel200?> ListModels(string accountId, string? task = null, string? author = null,
        bool? hideExperimental = null, int? page = null, int? perPage = null,
        CancellationToken cancellationToken = default);

    /// <summary>Gets the catalog model matching the supplied model name.</summary>
    ValueTask<WorkersAiSearchModel200Member1_result?> GetModel(string accountId, string modelName,
        CancellationToken cancellationToken = default);

    /// <summary>Enumerates catalog models across all result pages.</summary>
    IAsyncEnumerable<WorkersAiSearchModel200Member1_result> EnumerateModels(string accountId, string? search = null,
        string? task = null, string? author = null, bool? hideExperimental = null, int pageSize = 100,
        CancellationToken cancellationToken = default);

    /// <summary>Gets the available Workers AI tasks.</summary>
    ValueTask<WorkersAiSearchTask200?> ListTasks(string accountId, CancellationToken cancellationToken = default);

    /// <summary>Gets the available Workers AI model authors.</summary>
    ValueTask<WorkersAiSearchAuthor200?> ListAuthors(string accountId, CancellationToken cancellationToken = default);
}
