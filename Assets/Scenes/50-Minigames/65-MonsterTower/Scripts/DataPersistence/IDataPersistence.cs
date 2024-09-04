using Scenes._50_Minigames._65_MonsterTower.Scripts.DataPersistence.Data;

//Interface an object can use to save and load data. 
namespace Scenes._50_Minigames._65_MonsterTower.Scripts.DataPersistence
{
    public interface IDataPersistence
    {

        void LoadData(GameData data);
        void SaveData(ref GameData data);

    }
}
