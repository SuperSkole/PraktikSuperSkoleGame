using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace _99_Legacy.Menu
{
    public class SeeChoosenPlayer : MonoBehaviour
    {
        [SerializeField] private GameObject[] ShownplayerParts;
        [SerializeField] private Sprite[] SimpleMonsterParts;
        [SerializeField] private Sprite[] PengeByMonsterParts;
        [SerializeField] private Button finalButton;
        [SerializeField] private CinemachineVirtualCamera mainCamera;

        private void Start()
        {
            finalButton.onClick.AddListener(() => findPlayer());
        }
        /// <summary>
        /// The methode finds the player, doesnt matter which player is loading in as long as the tag is Player
        /// Used for creating all the interactions event the player needs
        /// </summary>
        private void findPlayer()
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
            {
                obj.GetComponent<PlayerWorldMovement>().GenerateInteractions();
                mainCamera.Follow = obj.transform;
                mainCamera.LookAt = obj.transform;
            }
            else
            {
                Debug.Log("Cant Find obj with tag Player");
            }

        }
        //Maybe delete at a later date
        public void TakeINFO(string name)
        {
            switch (name)
            {
                case "Monster":
                    for (int i = 0; i < 3; i++)
                    {
                        ShownplayerParts[i].GetComponent<Image>().sprite = SimpleMonsterParts[i];
                    }
                    break;
                case "Girl":
                    for (int i = 0; i < 3; i++)
                    {
                        ShownplayerParts[i].GetComponent<Image>().sprite = PengeByMonsterParts[i];
                    }
                    break;
            }
        }

    }
}
