using Soenneker.Cloudflare.OpenApiClient.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Workers.Ai.Abstract;

public partial interface ICloudflareWorkersAiUtil
{
    /// <summary>
    /// Lists models in the Workers AI catalog.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="task">Asynchronous operation to run.</param>
    /// <param name="author">Author for the list models operation.</param>
    /// <param name="hideExperimental">Hide Experimental for the list models operation.</param>
    /// <param name="page">Browser page to inspect or control.</param>
    /// <param name="perPage">Per Page for the list models operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Search Model.</returns>
    ValueTask<WorkersAiSearchModel200?> ListModels(string accountId, string? task = null, string? author = null,
        bool? hideExperimental = null, int? page = null, int? perPage = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the catalog model matching the supplied model name.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="modelName">Name of the model to use.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Search Model200 Member1_result.</returns>
    ValueTask<WorkersAiSearchModel200Member1_result?> GetModel(string accountId, string modelName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enumerates catalog models across all result pages.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="search">Search text or criteria to apply.</param>
    /// <param name="task">Asynchronous operation to run.</param>
    /// <param name="author">Author for the enumerate models operation.</param>
    /// <param name="hideExperimental">Hide Experimental for the enumerate models operation.</param>
    /// <param name="pageSize">Maximum number of items to request per page.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>The resulting async Enumerable.</returns>
    IAsyncEnumerable<WorkersAiSearchModel200Member1_result> EnumerateModels(string accountId, string? search = null,
        string? task = null, string? author = null, bool? hideExperimental = null, int pageSize = 100,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the available Workers AI tasks.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Search Task.</returns>
    ValueTask<WorkersAiSearchTask200?> ListTasks(string accountId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the available Workers AI model authors.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Search Author.</returns>
    ValueTask<WorkersAiSearchAuthor200?> ListAuthors(string accountId, CancellationToken cancellationToken = default);
}
