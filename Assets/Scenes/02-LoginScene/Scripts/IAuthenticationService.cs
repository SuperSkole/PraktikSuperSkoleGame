using System.Threading.Tasks;

namespace Scenes._02_LoginScene.Scripts
{
    // Interface for authentication services
    public interface IAuthenticationService
    {
        Task SignInAsync();
        Task SignOutAsync();
    }
}