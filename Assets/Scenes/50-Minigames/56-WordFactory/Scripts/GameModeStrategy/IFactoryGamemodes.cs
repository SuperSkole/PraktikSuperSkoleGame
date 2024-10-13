using CORE.Scripts;
using Scenes._50_Minigames._56_WordFactory.Scripts.Managers;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.GameModeStrategy
{
    public interface IFactoryGamemodes : IGenericGameMode
    {
        public void SetCorrectAnswer(string str, WordFactoryGameManager factoryGameManager);        
    }
}
