namespace Soenneker.Cloudflare.Workers.Ai.Dtos;

/// <summary>
/// Represents a message sent to a conversational Workers AI model.
/// </summary>
public sealed class CloudflareWorkersAiMessage
{
    /// <summary>
    /// The message role, such as <c>system</c>, <c>user</c>, or <c>assistant</c>.
    /// </summary>
    public required string Role { get; init; }

    /// <summary>
    /// The textual message content.
    /// </summary>
    public required string Content { get; init; }
}
