[System.Serializable]
public class SkinData
{
    public string Skintype;
    public string SkinName;
    public bool isPurchased;
    public bool isEquipped;

    public SkinData(string skintype, string skinName, bool isPurchased, bool isEquipped)
    {
        Skintype = skintype;
        SkinName = skinName;
        isPurchased = isPurchased;
        isEquipped = isEquipped;
    }
}