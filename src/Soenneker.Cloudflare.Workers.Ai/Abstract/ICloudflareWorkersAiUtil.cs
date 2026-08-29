using Soenneker.Cloudflare.OpenApiClient.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Workers.Ai.Abstract;

/// <summary>
/// A utility for working with Cloudflare Workers AI.
/// </summary>
public partial interface ICloudflareWorkersAiUtil
{
    /// <summary>
    /// Runs a Workers AI model on demand.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="modelName">The full model name, such as <c>@cf/meta/llama-3.2-3b-instruct</c>.</param>
    /// <param name="input">The model-specific input.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Post Run Model.</returns>
    ValueTask<WorkersAiPostRunModel200?> Run(string accountId, string modelName, WorkersAiPostRunModel input,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches the Workers AI model catalog.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="search">Optional free-text search.</param>
    /// <param name="task">Optional task name filter.</param>
    /// <param name="author">Optional model author filter.</param>
    /// <param name="hideExperimental">Whether experimental models should be omitted.</param>
    /// <param name="page">Optional page number.</param>
    /// <param name="perPage">Optional number of results per page.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Search Model.</returns>
    ValueTask<WorkersAiSearchModel200?> SearchModels(string accountId, string? search = null, string? task = null,
        string? author = null, bool? hideExperimental = null, int? page = null, int? perPage = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the input and output schema for a Workers AI model.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="modelName">Name of the model to use.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Get Model Schema.</returns>
    ValueTask<WorkersAiGetModelSchema200?> GetModelSchema(string accountId, string modelName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the available Workers AI tasks.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Search Task.</returns>
    ValueTask<WorkersAiSearchTask200?> SearchTasks(string accountId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the available Workers AI model authors.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Search Author.</returns>
    ValueTask<WorkersAiSearchAuthor200?> SearchAuthors(string accountId, CancellationToken cancellationToken = default);

}
