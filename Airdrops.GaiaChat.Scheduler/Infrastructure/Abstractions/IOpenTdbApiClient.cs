using Airdrops.Nodes.Domain.Dtos;
using Refit;

namespace Airdrops.Nodes.Infrastructure.Abstractions
{
    public interface IOpenTdbApiClient
    {
        [Get("/api.php")]
        Task<QuestionsReponse> GetQuestions([Query] int amount);
    }
}
