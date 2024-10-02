using Scenes._11_PlayerHouseScene.script.SaveData;

namespace LoadSave
{
    [System.Serializable]
    public class HouseDataDTO : IDataTransferObject
    {
       // public SerializableGridData FloorData { get; set; }
       // public SerializableGridData FurnitureData { get; set; }
        public SerializableGridData SavedGridData { get; set; }

        public HouseDataDTO(SerializableGridData SavedGridData)
        {
            this.SavedGridData = SavedGridData;
        }
        //public HouseDataDTO(SerializableGridData floorData, SerializableGridData furnitureData)
        //{
        //    FloorData = floorData;
        //    FurnitureData = furnitureData;
        //}
    }
}