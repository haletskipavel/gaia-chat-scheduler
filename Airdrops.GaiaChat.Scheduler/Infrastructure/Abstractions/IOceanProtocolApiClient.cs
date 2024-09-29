using Airdrops.GaiaChat.Scheduler.Domain.Dtos.Ocean;
using Refit;

namespace Airdrops.Nodes.Infrastructure.Abstractions
{
    public interface IOceanProtocolApiClient
    {
        [Get("/nodes")]
        [Headers("accept: application/json", "Content-Type: application/json")]
        Task<RootResponse> GetNodesAsync(
            [AliasAs("search")] string search,
            [AliasAs("page")] int page = 1,
            [AliasAs("size")] int size = 1);
    }
}
