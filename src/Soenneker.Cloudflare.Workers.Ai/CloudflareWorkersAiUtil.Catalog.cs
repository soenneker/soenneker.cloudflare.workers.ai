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
    public ValueTask<WorkersAiSearchModel200?> ListModels(string accountId, string? task = null, string? author = null,
        bool? hideExperimental = null, int? page = null, int? perPage = null, CancellationToken cancellationToken = default) =>
        SearchModels(accountId, task: task, author: author, hideExperimental: hideExperimental, page: page, perPage: perPage,
            cancellationToken: cancellationToken);

    public async ValueTask<WorkersAiSearchModel200Member1_result?> GetModel(string accountId, string modelName,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(modelName);
        WorkersAiSearchModel200? response = await SearchModels(accountId, search: modelName, perPage: 100, cancellationToken: cancellationToken);
        List<WorkersAiSearchModel200Member1_result>? models = response?.WorkersAiSearchModel200Member1?.Result;
        if (models is null)
            return null;

        return models.FirstOrDefault(model => HasModelName(model, modelName));
    }

    public async IAsyncEnumerable<WorkersAiSearchModel200Member1_result> EnumerateModels(string accountId, string? search = null,
        string? task = null, string? author = null, bool? hideExperimental = null, int pageSize = 100,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);
        var page = 1;

        while (true)
        {
            WorkersAiSearchModel200? response = await SearchModels(accountId, search, task, author, hideExperimental, page, pageSize,
                cancellationToken);
            List<WorkersAiSearchModel200Member1_result>? models = response?.WorkersAiSearchModel200Member1?.Result;

            if (models is null || models.Count == 0)
                yield break;

            foreach (WorkersAiSearchModel200Member1_result model in models)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return model;
            }

            if (models.Count < pageSize)
                yield break;

            page++;
        }
    }

    public ValueTask<WorkersAiSearchTask200?> ListTasks(string accountId, CancellationToken cancellationToken = default) =>
        SearchTasks(accountId, cancellationToken);

    public ValueTask<WorkersAiSearchAuthor200?> ListAuthors(string accountId, CancellationToken cancellationToken = default) =>
        SearchAuthors(accountId, cancellationToken);

    private static bool HasModelName(WorkersAiSearchModel200Member1_result model, string modelName)
    {
        foreach (string key in new[] { "name", "id", "model" })
        {
            if (model.AdditionalData.TryGetValue(key, out object? value) && value is string text &&
                string.Equals(text, modelName, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}
