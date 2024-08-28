using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClothingManager : MonoBehaviour
{
    //IMAGES FOR SHOPITEM
    [SerializeField] Image briller;
    [SerializeField] Image tryllehat;
    [SerializeField] Image tophat;
    [SerializeField] Image slips;
    [SerializeField] Image halstorklade;
    [SerializeField] Image ballerinaskort;

    public static ClothingManager Instance;
    public List<ClothInfo> AvailableClothesList = new List<ClothInfo>();

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void InitializeClothes()
    {
        // SHOPITEMS
        AvailableClothesList.Add(new ClothInfo { ID = 1, image = briller, Price = 100, Name = "Briller" });
        AvailableClothesList.Add(new ClothInfo { ID = 2, image = tryllehat, Price = 200, Name = "Tryllehat" });
        AvailableClothesList.Add(new ClothInfo { ID = 3, image = tophat, Price = 300, Name = "Tophat" });
        AvailableClothesList.Add(new ClothInfo { ID = 4, image = slips, Price = 150, Name = "Slips" });
        AvailableClothesList.Add(new ClothInfo { ID = 5, image = halstorklade, Price = 180, Name = "Halstørklæde" });
        AvailableClothesList.Add(new ClothInfo { ID = 6, image = ballerinaskort, Price = 250, Name = "Ballerinaskørt" });
    }
    public List<ClothInfo> CipherList(List<int> checkList)
    {
        //the list we send back
        List<ClothInfo> matchingClothes = new List<ClothInfo>();

        foreach (int id in checkList)
        {
            // match check
            ClothInfo matchingItem = AvailableClothesList.Find(cloth => cloth.ID == id);

            // match found
            if (matchingItem != null)
            {
                matchingClothes.Add(matchingItem);
            }
        }

        return matchingClothes;
    }

  

}
