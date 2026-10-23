using Soenneker.Cloudflare.OpenApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Workers.Ai;

public sealed partial class CloudflareWorkersAiUtil
{
    private static readonly string[] _modelNameKeys = ["name", "id", "model"];

    public ValueTask<WorkersAiSearchModel200Response?> ListModels(string accountId, string? task = null, string? author = null,
        bool? hideExperimental = null, int? page = null, int? perPage = null, CancellationToken cancellationToken = default) =>
        SearchModels(accountId, task: task, author: author, hideExperimental: hideExperimental, page: page, perPage: perPage,
            cancellationToken: cancellationToken);

    public async ValueTask<WorkersAiSearchModel200ResponseAnyOf1ResultItem?> GetModel(string accountId, string modelName,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(modelName);
        WorkersAiSearchModel200Response? response = await SearchModels(accountId, search: modelName, perPage: 100, cancellationToken: cancellationToken);
        List<WorkersAiSearchModel200ResponseAnyOf1ResultItem>? models = GetModels(response);
        if (models is null)
            return null;

        return models.FirstOrDefault(model => HasModelName(model, modelName));
    }

    public async IAsyncEnumerable<WorkersAiSearchModel200ResponseAnyOf1ResultItem> EnumerateModels(string accountId, string? search = null,
        string? task = null, string? author = null, bool? hideExperimental = null, int pageSize = 100,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);
        var page = 1;

        while (true)
        {
            WorkersAiSearchModel200Response? response = await SearchModels(accountId, search, task, author, hideExperimental, page, pageSize,
                cancellationToken);
            List<WorkersAiSearchModel200ResponseAnyOf1ResultItem>? models = GetModels(response);

            if (models is null || models.Count == 0)
                yield break;

            foreach (WorkersAiSearchModel200ResponseAnyOf1ResultItem model in models)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return model;
            }

            if (models.Count < pageSize)
                yield break;

            page++;
        }
    }

    public ValueTask<WorkersAiSearchTask200Response?> ListTasks(string accountId, CancellationToken cancellationToken = default) =>
        SearchTasks(accountId, cancellationToken);

    public ValueTask<WorkersAiSearchAuthor200Response?> ListAuthors(string accountId, CancellationToken cancellationToken = default) =>
        SearchAuthors(accountId, cancellationToken);

    private static List<WorkersAiSearchModel200ResponseAnyOf1ResultItem>? GetModels(WorkersAiSearchModel200Response? response) =>
        response?.WorkersAiSearchModel200ResponseAnyOf1?.Result ??
        response?.WorkersAiSearchModel200ResponseAnyOf2?.Data?.Select(item => new WorkersAiSearchModel200ResponseAnyOf1ResultItem
        {
            AdditionalData = new Dictionary<string, object>(item.AdditionalData)
        }).ToList();
    private static bool HasModelName(WorkersAiSearchModel200ResponseAnyOf1ResultItem model, string modelName)
    {
        foreach (string key in _modelNameKeys)
        {
            if (model.AdditionalData.TryGetValue(key, out object? value) && value is string text &&
                string.Equals(text, modelName, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}
