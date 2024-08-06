using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public class ShopSkinManagement : MonoBehaviour
{
    public CusParts girlSkins = new CusParts();// Assign these in the inspector
    public CusParts monsterSkins = new CusParts();// Assign these in the inspector

    public IconSpriteMapper iconSpriteMapper;
    [Header("De skal følge same rækkefølge")]
    //These list have to follow the same order of sprites
    public List<Sprite> icons; // Assign these in the inspector
    public List<Sprite> sprites; // Assign these in the inspector

    public void PrintList()
    {
        Debug.Log(girlSkins.head[0].purchased + " " + girlSkins.head[0].equipped);
    }
    public void StartMapping()
    {
        // Ensure the iconSpriteMapper is assigned
        if (iconSpriteMapper != null)
        {
            iconSpriteMapper.InitializeMappings(icons, sprites);
        }
        else
        {
            Debug.LogError("ShopSkinManagement/StartMapping/IconSpriteMapper not assigned.");
        }
    }
   
    //Extracts the skin data from CusParts and returns a list of SkinData.
    private List<SkinData> GetSkinData(CusParts cusParts)
    {
        List<SkinData> skinDataList = new List<SkinData>();

        foreach (var skin in cusParts.head)
        {
            skinDataList.Add(new SkinData("Head",skin.skin.name, skin.purchased, skin.equipped));
        }
        foreach (var skin in cusParts.torso)
        {
            skinDataList.Add(new SkinData("Body", skin.skin.name, skin.purchased, skin.equipped));
        }
        foreach (var skin in cusParts.legs)
        {
            skinDataList.Add(new SkinData("Legs", skin.skin.name, skin.purchased, skin.equipped));
        }

        return skinDataList;
    }
}
