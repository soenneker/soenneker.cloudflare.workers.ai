[![](https://img.shields.io/nuget/v/soenneker.cloudflare.workers.ai.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cloudflare.workers.ai/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cloudflare.workers.ai/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.cloudflare.workers.ai/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.cloudflare.workers.ai.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cloudflare.workers.ai/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Cloudflare.Workers.Ai
### A utility for working with Cloudflare Workers AI.

This library uses the generated `Soenneker.Cloudflare.OpenApiClient` (Kiota) client and provides dependency-injection-friendly operations for running models, searching the model catalog, and retrieving model schemas, tasks, and authors.

## Installation

```
dotnet add package Soenneker.Cloudflare.Workers.Ai
```

## Registration

```csharp
services.AddCloudflareWorkersAiUtilAsSingleton();
```

Configure the Cloudflare API token:

```json
{
  "Cloudflare": {
    "ApiKey": "your-api-token"
  }
}
```

## Usage

```csharp
var input = new WorkersAiPostRunModel
{
    WorkersAiPostRunModelMember5 = new WorkersAiPostRunModelMember5
    {
        Prompt = "Explain edge computing in one sentence."
    }
};

WorkersAiPostRunModel200? response = await workersAi.Run(
    accountId,
    "@cf/meta/llama-3.2-3b-instruct",
    input,
    cancellationToken);
```

The generated request and response models remain public, so callers retain access to Cloudflare's complete typed payloads.

## Convenience operations

The utility also includes focused methods for common workloads:

```csharp
await workersAi.GenerateText(accountId, model, "Explain edge computing.", cancellationToken: cancellationToken);

await workersAi.Chat(accountId, model,
[
    new CloudflareWorkersAiMessage { Role = "system", Content = "Be concise." },
    new CloudflareWorkersAiMessage { Role = "user", Content = "What is Workers AI?" }
], cancellationToken: cancellationToken);

await workersAi.GenerateEmbeddings(accountId, embeddingModel,
    ["first document", "second document"], cancellationToken);
```

Additional convenience methods include `GenerateImage`, `Transcribe`, and `Translate`. `RunGeneric` exposes Cloudflare's generic model endpoint when a workload needs model-specific input not covered by these helpers.

## Streaming

`RunStreaming` yields the data payload of each server-sent event without buffering the complete response:

```csharp
await foreach (string data in workersAi.RunStreaming(accountId, model,
    new Dictionary<string, object?>
    {
        ["prompt"] = "Write a short story.",
        ["max_tokens"] = 500
    }, cancellationToken))
{
    // Each value is the raw JSON data payload from Cloudflare.
}
```

## Catalog and Markdown conversion

`ListModels`, `GetModel`, and `EnumerateModels` provide model catalog access, with `EnumerateModels` loading pages lazily. `ListTasks` and `ListAuthors` expose catalog metadata.

Files can be converted through `ConvertToMarkdown` using a file path, byte array, or stream. `GetSupportedMarkdownFormats` returns Cloudflare's currently supported formats.
