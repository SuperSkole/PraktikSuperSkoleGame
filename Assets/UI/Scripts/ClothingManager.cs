using System.Collections;
using System.Collections.Generic;
using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class ClothingManager : MonoBehaviour
    {
        //IMAGES FOR SHOPITEM
        [SerializeField] private Sprite briller;
        [SerializeField] private Sprite tryllehat;
        [SerializeField] private Sprite tophat;
        [SerializeField] private Sprite slips;
        [SerializeField] private Sprite halstorklade;
        [SerializeField] private Sprite ballerinaskort;

        [SerializeField] private Sprite rod;
        [SerializeField] private Sprite gron;
        [SerializeField] private Sprite gul;
        [SerializeField] private Sprite hvid;
        [SerializeField] private Sprite bla;
        [SerializeField] private Sprite pink;

        public static ClothingManager Instance;
        [SerializeField] public List<ClothInfo> AvailableClothesList = new List<ClothInfo>();

        private void Awake()
        {
            // Singleton
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeClothes();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void InitializeClothes()
        {
            // SHOPITEMS
            AvailableClothesList.Add(new ClothInfo { ID = 1, image = briller, Price = 50, Name = "Briller", SpineName = "Monster HEAD Briller" });
            AvailableClothesList.Add(new ClothInfo { ID = 2, image = tryllehat, Price = 75, Name = "Tryllehat", SpineName = "Monster HEAD Tryllehat" });
            AvailableClothesList.Add(new ClothInfo { ID = 3, image = tophat, Price = 100, Name = "Tophat", SpineName = "Monster HEAD Tophat" });

            AvailableClothesList.Add(new ClothInfo { ID = 4, image = slips, Price = 50, Name = "Slips", SpineName = "Monster MID Slips" });
            AvailableClothesList.Add(new ClothInfo { ID = 5, image = halstorklade, Price = 100, Name = "Halstørklæde", SpineName = "Monster MID Halstørklæde" });
            AvailableClothesList.Add(new ClothInfo { ID = 6, image = ballerinaskort, Price = 200, Name = "Ballerinaskørt", SpineName = "Monster MID Ballerinaskørt" });

            AvailableClothesList.Add(new ClothInfo { ID = 7, image = rod, Price = 20, Name = "Rød", SpineName = "red" });
            AvailableClothesList.Add(new ClothInfo { ID = 8, image = gron, Price = 20, Name = "Grøn", SpineName = "green" });
            AvailableClothesList.Add(new ClothInfo { ID = 9, image = bla, Price = 20, Name = "Blå", SpineName = "blue" });
            AvailableClothesList.Add(new ClothInfo { ID = 10, image = gul, Price = 20, Name = "Gul", SpineName = "orange" });
            AvailableClothesList.Add(new ClothInfo { ID = 11, image = hvid, Price = 20, Name = "Hvid", SpineName = "white" });
            AvailableClothesList.Add(new ClothInfo { ID = 12, image = pink, Price = 20, Name = "Pink", SpineName = "pink" });
        }
        public List<ClothInfo> CipherList(List<int> checkList)
        {
            // the list we send back
            List<ClothInfo> nonMatchingClothes = new List<ClothInfo>();

            foreach (ClothInfo cloth in AvailableClothesList)
            {
                // check if the cloth ID is not in the checkList
                if (!checkList.Contains(cloth.ID))
                {
                    // if not found, add to nonMatchingClothes
                    nonMatchingClothes.Add(cloth);
                }
            }

            return nonMatchingClothes;
        }

        public List<ClothInfo> WardrobeContent(List<int> checkList)
        {
            //list we send back
            List<ClothInfo> matchingClothes = new List<ClothInfo>();

            foreach (ClothInfo cloth in AvailableClothesList)
            {
                // check if the cloth ID is not in the checkList
                if (checkList.Contains(cloth.ID))
                {
                    // if not found, add to nonMatchingClothes
                    matchingClothes.Add(cloth);
                }
            }

            return matchingClothes;
        }


    }
}
