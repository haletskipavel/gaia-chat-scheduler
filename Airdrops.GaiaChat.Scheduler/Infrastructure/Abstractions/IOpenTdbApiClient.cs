using Airdrops.GaiaChat.Scheduler.Domain.Dtos.Question;
using Refit;

namespace Airdrops.Nodes.Infrastructure.Abstractions
{
    public interface IOpenTdbApiClient
    {
        [Get("/api.php")]
        Task<QuestionsReponse> GetQuestions([Query] int amount);
    }
}
