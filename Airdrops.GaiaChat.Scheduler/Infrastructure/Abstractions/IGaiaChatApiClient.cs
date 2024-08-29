using Airdrops.Nodes.Domain.Dtos;
using Refit;

namespace Airdrops.Nodes.Infrastructure.Abstractions
{
    public interface IGaiaChatApiClient
    {
        [Post("/v1/chat/completions")]
        [Headers("accept: application/json", "Content-Type: application/json")]
        Task<ChatCompletionResponse> GetChatCompletionAsync([Body] ChatCompletionRequest request);
    }
}
