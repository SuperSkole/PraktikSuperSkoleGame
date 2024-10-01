using CORE.Scripts;
using System.Collections;
using UnityEngine;

namespace Scenes._50_Minigames._67_WordProductionLine.Scripts
{
    public class CreateProductionLineBoxes : MonoBehaviour
    {

        [SerializeField]
        private GameObject botSpawnPoint;

        public bool isOn = true;

        [SerializeField]
        private ProductionLineObjectPool objectBoxPool;


        [SerializeField]
        private ProductionLineManager productionManager;



        private void Start()
        {
            StartCoroutine(WaitForFourSeconds());
        }




        /// <summary>
        /// creates letterboxes.
        /// </summary>
        private void CreateProductionLineLetterBox()
        {

            GameObject letterBox = objectBoxPool.GetPooledObject();

            if (letterBox != null)
            {
                if (Random.Range(0, 2) == 1)
                {
                    letterBox.transform.GetChild(0).gameObject.GetComponent<LetterBox>().GetLetter(productionManager.GetFixedCorrect());
                }
                else
                {
                    letterBox.transform.GetChild(0).gameObject.GetComponent<LetterBox>().GetLetter(productionManager.GetLetters());
                }
                
                letterBox.transform.position = botSpawnPoint.transform.position;
                letterBox.SetActive(true);
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
                    CreateProductionLineLetterBox();
                }
            }
        }


    }
}