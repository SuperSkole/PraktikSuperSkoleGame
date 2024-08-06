[System.Serializable]
public class SkinData
{
    public string Skintype;
    public string SkinName;
    public bool Purchased;
    public bool Equipped;

    public SkinData(string skintype, string skinName, bool purchased, bool equipped)
    {
        Skintype = skintype;
        SkinName = skinName;
        Purchased = purchased;
        Equipped = equipped;
    }
}