using System.Threading.Tasks;
using Soenneker.Cloudflare.Workers.Ai.Abstract;
using Soenneker.Tests.HostedUnit;
using System.Threading;

namespace Soenneker.Cloudflare.Workers.Ai.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class CloudflareWorkersAiUtilTests : HostedUnitTest
{
    private readonly ICloudflareWorkersAiUtil _util;

    public CloudflareWorkersAiUtilTests(Host host) : base(host)
    {
        _util = Resolve<ICloudflareWorkersAiUtil>(true);
    }

    [Test]
    public async ValueTask Resolves(CancellationToken cancellationToken)
    {
        await Assert.That(_util).IsNotNull();
    }
}
