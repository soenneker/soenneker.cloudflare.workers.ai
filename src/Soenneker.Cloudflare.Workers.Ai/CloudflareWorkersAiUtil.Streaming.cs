using Soenneker.Extensions.ValueTask;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Soenneker.Cloudflare.Workers.Ai;

public sealed partial class CloudflareWorkersAiUtil
{
    [RequiresUnreferencedCode("Arbitrary model inputs require preserved JSON metadata. Use the overload accepting JsonTypeInfo for AOT.")]
    [RequiresDynamicCode("Arbitrary model inputs can require runtime serialization code. Use the overload accepting JsonTypeInfo for AOT.")]
    public IAsyncEnumerable<string> RunStreaming(string accountId, string modelName, IReadOnlyDictionary<string, object?> input,
        CancellationToken cancellationToken = default) =>
        RunStreamingCore(accountId, modelName, input, static payload => JsonContent.Create(payload), cancellationToken);

    public IAsyncEnumerable<string> RunStreaming(string accountId, string modelName, IReadOnlyDictionary<string, object?> input,
        JsonTypeInfo<Dictionary<string, object?>> typeInfo, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(typeInfo);
        return RunStreamingCore(accountId, modelName, input, payload => JsonContent.Create(payload, typeInfo), cancellationToken);
    }

    private async IAsyncEnumerable<string> RunStreamingCore(string accountId, string modelName, IReadOnlyDictionary<string, object?> input,
        Func<Dictionary<string, object?>, HttpContent> createContent, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accountId);
        ArgumentException.ThrowIfNullOrWhiteSpace(modelName);
        ArgumentNullException.ThrowIfNull(input);

        var payload = new Dictionary<string, object?>(input) { ["stream"] = true };
        System.Net.Http.HttpClient httpClient = await _httpClientUtil.Get(cancellationToken).NoSync();
        var path = $"accounts/{Uri.EscapeDataString(accountId)}/ai/run/{Uri.EscapeDataString(modelName)}";

        using var request = new HttpRequestMessage(HttpMethod.Post, path);
        request.Content = createContent(payload);
        request.Headers.Accept.ParseAdd("text/event-stream");

        using HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Workers AI streaming request failed with status {(int) response.StatusCode}", null,
                response.StatusCode);
        }

        await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            if (!line.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                continue;

            ReadOnlySpan<char> data = line.AsSpan(5).TrimStart();
            if (data.Length == 0 || data.SequenceEqual("[DONE]"))
                continue;

            yield return data.ToString();
        }
    }
}
