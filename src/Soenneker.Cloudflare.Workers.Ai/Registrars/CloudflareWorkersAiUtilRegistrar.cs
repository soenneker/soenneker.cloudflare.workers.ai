using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Cloudflare.Utils.Client.Registrars;
using Soenneker.Cloudflare.Workers.Ai.Abstract;

namespace Soenneker.Cloudflare.Workers.Ai.Registrars;

/// <summary>
/// A utility for working with Cloudflare Workers AI.
/// </summary>
public static class CloudflareWorkersAiUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="ICloudflareWorkersAiUtil"/> as a singleton service. <para/>
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddCloudflareWorkersAiUtilAsSingleton(this IServiceCollection services)
    {
        services.AddCloudflareClientUtilAsSingleton().TryAddSingleton<ICloudflareWorkersAiUtil, CloudflareWorkersAiUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="ICloudflareWorkersAiUtil"/> as a scoped service. <para/>
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddCloudflareWorkersAiUtilAsScoped(this IServiceCollection services)
    {
        services.AddCloudflareClientUtilAsSingleton().TryAddScoped<ICloudflareWorkersAiUtil, CloudflareWorkersAiUtil>();

        return services;
    }
}
