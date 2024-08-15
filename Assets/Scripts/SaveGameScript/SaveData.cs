using System.Collections.Generic;

/// <summary>
/// This Class will hold the data thats neasecary to be saved and then converted to JSON
/// </summary>
[System.Serializable]
public class SaveDataDTO 
{
    //public CharacterController playerData;
    public string PlayerName;
    public string MonsterName;
    public int GoldAmount;
    public int XPAmount;
    public int PlayerLevel;

    public SavePlayerPosition SavedPlayerStartPostion;

    public SerializableColor HeadColor;
    public SerializableColor BodyColor;
    public SerializableColor LegColor;

    //public string CurrentHeadSpriteName;
    //public string CurrentBodySpriteName;
    //public string CurrentLegsSpriteName;

    //For storing purchased and equipped skin
    public List<SkinData> GirlPurchasedSkins = new List<SkinData>();
    public List<SkinData> MonsterPurchasedSkins = new List<SkinData>();

}
