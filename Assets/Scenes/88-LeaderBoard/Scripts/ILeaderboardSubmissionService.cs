using System.Threading.Tasks;

namespace Scenes._88_LeaderBoard.Scripts
{
    public interface ILeaderboardSubmissionService
    {
        Task SubmitMostWords(int wordCount, string name);
        Task SubmitMostLetters(int wordCount, string name);
        Task EnsureSignedIn();
    }
}