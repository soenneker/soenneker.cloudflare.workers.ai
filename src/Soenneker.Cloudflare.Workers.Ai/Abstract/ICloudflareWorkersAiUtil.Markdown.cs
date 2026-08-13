using Soenneker.Cloudflare.OpenApiClient.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Workers.Ai.Abstract;

public partial interface ICloudflareWorkersAiUtil
{
    /// <summary>Converts a file to Markdown.</summary>
    ValueTask<WorkersAiPostToMarkdown200?> ConvertToMarkdown(string accountId, string filePath,
        CancellationToken cancellationToken = default);

    /// <summary>Converts a byte payload to Markdown.</summary>
    ValueTask<WorkersAiPostToMarkdown200?> ConvertToMarkdown(string accountId, byte[] content, string fileName,
        string contentType = "application/octet-stream", CancellationToken cancellationToken = default);

    /// <summary>Converts a stream to Markdown. The caller owns the stream.</summary>
    ValueTask<WorkersAiPostToMarkdown200?> ConvertToMarkdown(string accountId, Stream content, string fileName,
        string contentType = "application/octet-stream", CancellationToken cancellationToken = default);

    /// <summary>Gets the file formats supported by Workers AI Markdown conversion.</summary>
    ValueTask<WorkersAiGetToMarkdownSupported200?> GetSupportedMarkdownFormats(string accountId,
        CancellationToken cancellationToken = default);
}
