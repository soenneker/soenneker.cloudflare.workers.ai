using Microsoft.Kiota.Abstractions;
using Soenneker.Cloudflare.OpenApiClient;
using Soenneker.Cloudflare.OpenApiClient.Models;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Workers.Ai;

public sealed partial class CloudflareWorkersAiUtil
{
    public async ValueTask<WorkersAiPostToMarkdown200?> ConvertToMarkdown(string accountId, string filePath,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        await using FileStream stream = File.OpenRead(filePath);
        return await ConvertToMarkdown(accountId, stream, Path.GetFileName(filePath), cancellationToken: cancellationToken);
    }

    public async ValueTask<WorkersAiPostToMarkdown200?> ConvertToMarkdown(string accountId, byte[] content, string fileName,
        string contentType = "application/octet-stream", CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(content);
        if (content.Length == 0)
            throw new ArgumentException("Content cannot be empty.", nameof(content));

        await using var stream = new MemoryStream(content, writable: false);
        return await ConvertToMarkdown(accountId, stream, fileName, contentType, cancellationToken);
    }

    public async ValueTask<WorkersAiPostToMarkdown200?> ConvertToMarkdown(string accountId, Stream content, string fileName,
        string contentType = "application/octet-stream", CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accountId);
        ArgumentNullException.ThrowIfNull(content);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);
        if (!content.CanRead)
            throw new ArgumentException("The content stream must be readable.", nameof(content));

        CloudflareOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();
        var multipartBody = new MultipartBody();
        multipartBody.AddOrReplacePart("files", contentType, content, fileName);
        return await client.Accounts[accountId].Ai.Tomarkdown.PostAsync(multipartBody, cancellationToken: cancellationToken).NoSync();
    }

    public async ValueTask<WorkersAiGetToMarkdownSupported200?> GetSupportedMarkdownFormats(string accountId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accountId);
        CloudflareOpenApiClient client = await _clientUtil.Get(cancellationToken).NoSync();
        return await client.Accounts[accountId].Ai.Tomarkdown.Supported.GetAsync(cancellationToken: cancellationToken).NoSync();
    }
}
