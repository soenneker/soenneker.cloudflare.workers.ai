using Soenneker.Extensions.ValueTask;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Soenneker.Cloudflare.Workers.Ai;

public sealed partial class CloudflareWorkersAiUtil
{
    public async IAsyncEnumerable<string> RunStreaming(string accountId, string modelName, IReadOnlyDictionary<string, object?> input,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accountId);
        ArgumentException.ThrowIfNullOrWhiteSpace(modelName);
        ArgumentNullException.ThrowIfNull(input);

        var payload = new Dictionary<string, object?>(input) { ["stream"] = true };
        System.Net.Http.HttpClient httpClient = await _httpClientUtil.Get(cancellationToken).NoSync();
        var path = $"accounts/{Uri.EscapeDataString(accountId)}/ai/run/{Uri.EscapeDataString(modelName)}";

        using var request = new HttpRequestMessage(HttpMethod.Post, path);
        request.Content = JsonContent.Create(payload);
        request.Headers.Accept.ParseAdd("text/event-stream");

        using HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Workers AI streaming request failed with status {(int) response.StatusCode}: {error}", null,
                response.StatusCode);
        }

        await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            if (!line.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                continue;

            string data = line[5..].TrimStart();
            if (data.Length == 0 || data == "[DONE]")
                continue;

            yield return data;
        }
    }
}
