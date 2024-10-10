using CORE;
using System.Collections;
using System.Collections.Generic;
using UI.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Scripts
{
    public class ClothingManager : PersistentSingleton<ClothingManager>
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

        [SerializeField] public List<ClothInfo> AvailableClothesList = new List<ClothInfo>();

        private void Start()
        {
            InitializeClothes();
        }

        private void InitializeClothes()
        {
            // SHOPITEMS
            AvailableClothesList.Add(new ClothInfo { ID = 1, image = briller, Price = 1, Name = "Briller", SpineName = "Monster HEAD Briller" });
            AvailableClothesList.Add(new ClothInfo { ID = 2, image = tryllehat, Price = 1, Name = "Tryllehat", SpineName = "Monster HEAD Tryllehat" });
            AvailableClothesList.Add(new ClothInfo { ID = 3, image = tophat, Price = 1, Name = "Tophat", SpineName = "Monster HEAD Tophat" });

            AvailableClothesList.Add(new ClothInfo { ID = 4, image = slips, Price = 1, Name = "Slips", SpineName = "Monster MID Slips" });
            AvailableClothesList.Add(new ClothInfo { ID = 5, image = halstorklade, Price = 1, Name = "Halst\u00f8rkl\u00e6de", SpineName = "Monster MID Halst\u00f8rkl\u00e6de" });
            AvailableClothesList.Add(new ClothInfo { ID = 6, image = ballerinaskort, Price = 1, Name = "Ballerinask\u00f8rt", SpineName = "Monster MID Ballerinask\u00f8rt" });

            AvailableClothesList.Add(new ClothInfo { ID = 7, image = rod, Price = 1, Name = "R\u00f8d", SpineName = "red" });
            AvailableClothesList.Add(new ClothInfo { ID = 8, image = gron, Price = 1, Name = "Gr\u00f8n", SpineName = "green" });
            AvailableClothesList.Add(new ClothInfo { ID = 9, image = bla, Price = 1, Name = "Bl\u00e5", SpineName = "blue" });
            AvailableClothesList.Add(new ClothInfo { ID = 10, image = gul, Price = 1, Name = "Gul", SpineName = "orange" });
            AvailableClothesList.Add(new ClothInfo { ID = 11, image = hvid, Price = 1, Name = "Hvid", SpineName = "white" });
            AvailableClothesList.Add(new ClothInfo { ID = 12, image = pink, Price = 1, Name = "Pink", SpineName = "pink" });
            //AvailableClothesList.Add(new ClothInfo { ID = 1, image = briller, Price = 50, Name = "Briller", SpineName = "Monster HEAD Briller" });
            //AvailableClothesList.Add(new ClothInfo { ID = 2, image = tryllehat, Price = 75, Name = "Tryllehat", SpineName = "Monster HEAD Tryllehat" });
            //AvailableClothesList.Add(new ClothInfo { ID = 3, image = tophat, Price = 100, Name = "Tophat", SpineName = "Monster HEAD Tophat" });

            //AvailableClothesList.Add(new ClothInfo { ID = 4, image = slips, Price = 50, Name = "Slips", SpineName = "Monster MID Slips" });
            //AvailableClothesList.Add(new ClothInfo { ID = 5, image = halstorklade, Price = 100, Name = "Halst\u00f8rkl\u00e6de", SpineName = "Monster MID Halst\u00f8rkl\u00e6de" });
            //AvailableClothesList.Add(new ClothInfo { ID = 6, image = ballerinaskort, Price = 200, Name = "Ballerinask\u00f8rt", SpineName = "Monster MID Ballerinask\u00f8rt" });

            //AvailableClothesList.Add(new ClothInfo { ID = 7, image = rod, Price = 20, Name = "R\u00f8d", SpineName = "red" });
            //AvailableClothesList.Add(new ClothInfo { ID = 8, image = gron, Price = 20, Name = "Gr\u00f8n", SpineName = "green" });
            //AvailableClothesList.Add(new ClothInfo { ID = 9, image = bla, Price = 20, Name = "Bl\u00e5", SpineName = "blue" });
            //AvailableClothesList.Add(new ClothInfo { ID = 10, image = gul, Price = 20, Name = "Gul", SpineName = "orange" });
            //AvailableClothesList.Add(new ClothInfo { ID = 11, image = hvid, Price = 20, Name = "Hvid", SpineName = "white" });
            //AvailableClothesList.Add(new ClothInfo { ID = 12, image = pink, Price = 20, Name = "Pink", SpineName = "pink" });
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
