using Microsoft.Extensions.Logging;
using Soenneker.Cloudflare.OpenApiClient;
using Soenneker.Cloudflare.OpenApiClient.Models;
using Soenneker.Cloudflare.HttpClient.Abstract;
using Soenneker.Cloudflare.Utils.Client.Abstract;
using Soenneker.Cloudflare.Workers.Ai.Abstract;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Workers.Ai;

/// <inheritdoc cref="ICloudflareWorkersAiUtil"/>
public sealed partial class CloudflareWorkersAiUtil : ICloudflareWorkersAiUtil
{
    private readonly ICloudflareClientUtil _clientUtil;
    private readonly ICloudflareHttpClient _httpClientUtil;
    private readonly ILogger<CloudflareWorkersAiUtil> _logger;

    public CloudflareWorkersAiUtil(ICloudflareClientUtil clientUtil, ICloudflareHttpClient httpClientUtil, ILogger<CloudflareWorkersAiUtil> logger)
    {
        _clientUtil = clientUtil;
        _httpClientUtil = httpClientUtil;
        _logger = logger;
    }

    public async ValueTask<WorkersAiPostRunModel200?> Run(string accountId, string modelName, WorkersAiPostRunModel input,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accountId);
        ArgumentException.ThrowIfNullOrWhiteSpace(modelName);
        ArgumentNullException.ThrowIfNull(input);

        _logger.LogInformation("Running Workers AI model {ModelName} in account {AccountId}", modelName, accountId);

        try
        {
            CloudflareOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();
            WorkersAiPostRunModel200? response = await client.Accounts[accountId].Ai.Run[modelName]
                .PostAsync(input, cancellationToken: cancellationToken)
                .NoSync();

            _logger.LogInformation("Successfully ran Workers AI model {ModelName}", modelName);
            return response;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to run Workers AI model {ModelName} in account {AccountId}", modelName, accountId);
            throw;
        }
    }

    public async ValueTask<WorkersAiSearchModel200?> SearchModels(string accountId, string? search = null, string? task = null,
        string? author = null, bool? hideExperimental = null, int? page = null, int? perPage = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accountId);

        _logger.LogInformation("Searching Workers AI models in account {AccountId}", accountId);

        try
        {
            CloudflareOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();
            WorkersAiSearchModel200? response = await client.Accounts[accountId].Ai.Models.Search.GetAsync(config =>
            {
                config.QueryParameters.Search = search;
                config.QueryParameters.Task = task;
                config.QueryParameters.Author = author;
                config.QueryParameters.HideExperimental = hideExperimental;
                config.QueryParameters.Page = page;
                config.QueryParameters.PerPage = perPage;
            }, cancellationToken).NoSync();

            _logger.LogInformation("Successfully searched Workers AI models");
            return response;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to search Workers AI models in account {AccountId}", accountId);
            throw;
        }
    }

    public async ValueTask<WorkersAiGetModelSchema200?> GetModelSchema(string accountId, string modelName,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accountId);
        ArgumentException.ThrowIfNullOrWhiteSpace(modelName);

        _logger.LogInformation("Getting schema for Workers AI model {ModelName} in account {AccountId}", modelName, accountId);

        try
        {
            CloudflareOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();
            WorkersAiGetModelSchema200? response = await client.Accounts[accountId].Ai.Models.Schema.GetAsync(config =>
            {
                config.QueryParameters.Model = modelName;
            }, cancellationToken).NoSync();

            _logger.LogInformation("Successfully retrieved schema for Workers AI model {ModelName}", modelName);
            return response;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to get schema for Workers AI model {ModelName} in account {AccountId}", modelName, accountId);
            throw;
        }
    }

    public async ValueTask<WorkersAiSearchTask200?> SearchTasks(string accountId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accountId);

        _logger.LogInformation("Getting Workers AI tasks for account {AccountId}", accountId);

        try
        {
            CloudflareOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();
            WorkersAiSearchTask200? response = await client.Accounts[accountId].Ai.Tasks.Search
                .GetAsync(cancellationToken: cancellationToken)
                .NoSync();

            _logger.LogInformation("Successfully retrieved Workers AI tasks");
            return response;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to get Workers AI tasks for account {AccountId}", accountId);
            throw;
        }
    }

    public async ValueTask<WorkersAiSearchAuthor200?> SearchAuthors(string accountId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accountId);

        _logger.LogInformation("Getting Workers AI authors for account {AccountId}", accountId);

        try
        {
            CloudflareOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();
            WorkersAiSearchAuthor200? response = await client.Accounts[accountId].Ai.Authors.Search
                .GetAsync(cancellationToken: cancellationToken)
                .NoSync();

            _logger.LogInformation("Successfully retrieved Workers AI authors");
            return response;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to get Workers AI authors for account {AccountId}", accountId);
            throw;
        }
    }
}
