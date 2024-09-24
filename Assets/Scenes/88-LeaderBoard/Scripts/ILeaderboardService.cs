using System.Threading.Tasks;

namespace Scenes._88_LeaderBoard.Scripts
{
    public interface ILeaderboardService
    {
        Task SubmitMostWords(int wordCount);
        Task SubmitMostLetters(int wordCount);
    }
}