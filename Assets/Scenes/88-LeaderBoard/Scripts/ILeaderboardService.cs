using System.Threading.Tasks;

namespace Scenes._88_LeaderBoard.Scripts
{
    public interface ILeaderboardService
    {
        Task SubmitMostWords(int wordCount);
        Task SubmitMostGold(int goldAmount);
        Task SubmitHighestLevel(int level);
    }
}