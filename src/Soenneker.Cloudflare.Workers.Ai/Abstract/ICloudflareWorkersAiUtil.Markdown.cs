using Soenneker.Cloudflare.OpenApiClient.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Workers.Ai.Abstract;

/// <summary>
/// Defines Markdown conversion operations backed by Cloudflare Workers AI.
/// </summary>
public partial interface ICloudflareWorkersAiUtil
{
    /// <summary>
    /// Converts a file to Markdown.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="filePath">Path of the file to use.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Post To Markdown.</returns>
    ValueTask<WorkersAiPostToMarkdown200?> ConvertToMarkdown(string accountId, string filePath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Converts a byte payload to Markdown.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="content">Content to render, store, or send.</param>
    /// <param name="fileName">Name of the target file.</param>
    /// <param name="contentType">Media type describing the supplied content.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Post To Markdown.</returns>
    ValueTask<WorkersAiPostToMarkdown200?> ConvertToMarkdown(string accountId, byte[] content, string fileName,
        string contentType = "application/octet-stream", CancellationToken cancellationToken = default);

    /// <summary>
    /// Converts a stream to Markdown. The caller owns the stream.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="content">Content to render, store, or send.</param>
    /// <param name="fileName">Name of the target file.</param>
    /// <param name="contentType">Media type describing the supplied content.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Post To Markdown.</returns>
    ValueTask<WorkersAiPostToMarkdown200?> ConvertToMarkdown(string accountId, Stream content, string fileName,
        string contentType = "application/octet-stream", CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the file formats supported by Workers AI Markdown conversion.
    /// </summary>
    /// <param name="accountId">Identifier of the target account.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested workers Ai Get To Markdown Supported.</returns>
    ValueTask<WorkersAiGetToMarkdownSupported200?> GetSupportedMarkdownFormats(string accountId,
        CancellationToken cancellationToken = default);
}
