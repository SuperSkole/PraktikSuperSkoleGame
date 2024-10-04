using Scenes._11_PlayerHouseScene.script.SaveData;

namespace LoadSave
{
    [System.Serializable]
    public class HouseDataDTO : IDataTransferObject
    {
        public SerializableGridData SavedGridData { get; set; }

        public HouseDataDTO(SerializableGridData SavedGridData)
        {
            this.SavedGridData = SavedGridData;
        }
    }
}