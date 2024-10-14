using CORE.Scripts;
using System.Collections;
using Scenes._50_Minigames._67_WordProductionLine.Scripts;
using UnityEngine;

namespace Scenes._50_Minigames._67_WordProductionLine.Scripts
{

    public class CreateImageBox : MonoBehaviour
    {
        [SerializeField]
        private GameObject topSpawnPoint;

        public bool isOn = true;

        [SerializeField]
        private ProductionLineObjectPool objectBoxPool;

        [SerializeField]
        private ProductionLineController productionController;




        private void Start()
        {
            StartCoroutine(WaitForFourSeconds());
        }

        /// <summary>
        /// Creates ImageBoxes
        /// </summary>
        private void CreateProductionLineImageBox()
        {

            GameObject imageBox = objectBoxPool.GetPooledObject();

            if (imageBox != null)
            {
                string randoWord = productionController.GetImages();
                Texture2D randoImg = ImageManager.GetImageFromWord(randoWord);
                imageBox.transform.GetChild(0).gameObject.GetComponent<ImageBox>().GetImage(randoImg);
                imageBox.transform.position = topSpawnPoint.transform.position;
                imageBox.SetActive(true);
            }
        }

        /// <summary>
        /// waits 4 seconds...
        /// </summary>
        IEnumerator WaitForFourSeconds()
        {
            while (true)
            {


                // Wait for 4 seconds
                yield return new WaitForSeconds(4);

                if (isOn)
                {
                    CreateProductionLineImageBox();
                }
            }
        }
    }
}
