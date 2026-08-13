using System.Threading.Tasks;
using Soenneker.Cloudflare.Workers.Ai.Abstract;
using Soenneker.Tests.HostedUnit;

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
    public async Task Resolves()
    {
        await Assert.That(_util).IsNotNull();
    }
}
