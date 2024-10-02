using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using Scenes._11_PlayerHouseScene.script.HouseScripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class UIInvetoryManager : MonoBehaviour
{
    private PlayerData spawnedPlayerData;
    [SerializeField] private ObjectsDataBaseSO objectData;

    [SerializeField] List<Sprite> spritesForIcons;
    public List<FurnitureItemsInfo> ListOfFurniture = new();
    Dictionary<int, Sprite> DicOfSpritesForItems = new Dictionary<int, Sprite>();
    Dictionary<int, int> furnitureCount = new Dictionary<int, int>();// Dictionary to count occurrences of each furniture ID

    [SerializeField] Transform contentTransform;
    [SerializeField] GameObject itemTemplate;

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadFurnitureAmount()
    {
        DicOfSpritesForItems.Add(0, spritesForIcons[0]);
        DicOfSpritesForItems.Add(1, spritesForIcons[1]);
        DicOfSpritesForItems.Add(2, spritesForIcons[2]);
        DicOfSpritesForItems.Add(3, spritesForIcons[3]);
        DicOfSpritesForItems.Add(4, spritesForIcons[4]);
        DicOfSpritesForItems.Add(5, spritesForIcons[5]);
        DicOfSpritesForItems.Add(6, spritesForIcons[6]);
        DicOfSpritesForItems.Add(7, spritesForIcons[7]);
        DicOfSpritesForItems.Add(8, spritesForIcons[8]);

        spawnedPlayerData = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>();


        // Count how many of each furniture ID we have
        foreach (var id in spawnedPlayerData.ListOfFurniture)
        {
            if (furnitureCount.ContainsKey(id))
            {
                furnitureCount[id]++;
            }
            else
            {
                furnitureCount[id] = 1;
            }
        }

        int tmp = 0;
        // Spawn a single instance for each unique furniture ID
        foreach (var item in furnitureCount)
        {
            int id = item.Key;
            int count = item.Value;

            // Add furniture item to the list
            FurnitureItemsInfo itemInfo = new FurnitureItemsInfo(id, ReturnRightSprite(id), itemTemplate);

            // Instantiate a new UI item
            var spawnedItem = Instantiate(itemInfo.gameObject, contentTransform);
            spawnedItem.name = $"Items ({tmp})";
            tmp++;

            // Set sprite and item details
            spawnedItem.GetComponent<Image>().sprite = itemInfo.sprite;
            spawnedItem.GetComponent<PlaceableButtons>().FloorType = ReturnFloorType(itemInfo.ID);
            spawnedItem.GetComponent<PlaceableButtons>().ID = itemInfo.ID;

            // Display the count of this item (if more than one is owned)
            if (count > 1)
            {
                // Assuming there's a Text component to show the item count (you'll need to add one in the prefab)
                spawnedItem.GetComponent<PlaceableButtons>().StartUpValues(count);
            }
            itemInfo.gameObject = spawnedItem;
            ListOfFurniture.Add(itemInfo);

        }
    }
    public void AddFuritureBackToPile(int itemID)
    {
        //Check if there allready is an item
        foreach (var item in ListOfFurniture)
        {
            if (item.ID == itemID)
            {
                item.gameObject.GetComponent<PlaceableButtons>().ChangeValue(1);
                break;
            }
        }

        //If there is no Item 7
        if (furnitureCount.ContainsKey(itemID))
        {
            furnitureCount[itemID]++;
        }
        else
        {
            furnitureCount[itemID] = 1;
        }
        int count = furnitureCount[itemID];

        FurnitureItemsInfo itemInfo = new FurnitureItemsInfo(itemID, ReturnRightSprite(itemID), itemTemplate);
        var spawnedItem = Instantiate(itemInfo.gameObject, contentTransform);
        var nameCount = Convert.ToInt32(spawnedItem.name.Split("()"));
        spawnedItem.name = $"Items ({nameCount})";

        // Set sprite and item details
        spawnedItem.GetComponent<Image>().sprite = itemInfo.sprite;
        spawnedItem.GetComponent<PlaceableButtons>().FloorType = ReturnFloorType(itemInfo.ID);
        spawnedItem.GetComponent<PlaceableButtons>().ID = itemInfo.ID;

        // Display the count of this item (if more than one is owned)
        if (count > 1)
        {
            // Assuming there's a Text component to show the item count (you'll need to add one in the prefab)
            spawnedItem.GetComponent<PlaceableButtons>().StartUpValues(count);
        }
        itemInfo.gameObject = spawnedItem;
        ListOfFurniture.Add(itemInfo);
    }
    private EnumFloorDataType ReturnFloorType(int id)
    {
        foreach (var item in objectData.objectData)
        {
            if (item.ID == id)
            {
                return item.ObjectType;
            }
        }
        return EnumFloorDataType.Rug;
    }
    public void SaveFurnitureAmount()
    {
        List<int> tmpList = new List<int>();
        foreach (var item in ListOfFurniture)
        {
            tmpList.Add(item.ID);
        }
        spawnedPlayerData.ListOfFurniture = tmpList;
    }
    public void ModifyFurnitureAmount(int itemID)
    {
        foreach (var item in ListOfFurniture)
        {
            if (item.ID == itemID)
            {
                if (item.gameObject.GetComponent<PlaceableButtons>().amount == 1)
                {
                    item.gameObject.GetComponent<PlaceableButtons>().ChangeValue(-1);
                    ListOfFurniture.Remove(item);
                    furnitureCount.Remove(item.ID);
                    break;
                }
                else
                {
                    furnitureCount[itemID]--;
                    item.gameObject.GetComponent<PlaceableButtons>().ChangeValue(-1);
                    break;
                }
            }
        }
    }
    public int ReturnAmountOfRemaingItems(int itemID)
    {
        foreach (var item in ListOfFurniture)
        {
            if (item.ID == itemID)
            {
                return item.gameObject.GetComponent<PlaceableButtons>().amount;
            }
        }
        return 0;
    }
    private Sprite ReturnRightSprite(int id)
    {
        if (DicOfSpritesForItems.ContainsKey(id))
        {
            return DicOfSpritesForItems[id];
        }
        return null;
    }

}
[Serializable]
public class FurnitureItemsInfo
{
    [SerializeField] public int ID;
    [SerializeField] public Sprite sprite;
    [SerializeField] public GameObject gameObject;

    public FurnitureItemsInfo(int iD, Sprite sprite, GameObject prefab)
    {
        ID = iD;
        this.sprite = sprite;
        this.gameObject = prefab;
    }
}
