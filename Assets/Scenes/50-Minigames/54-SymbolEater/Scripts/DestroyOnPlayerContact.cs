using UnityEngine;

namespace Scenes._50_Minigames._54_SymbolEater.Scripts
{


    /// <summary>
    /// Used to destroy a gameobject on contact with the player
    /// </summary>
    public class DestroyOnPlayerContact : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Destroys the object when it overlaps with the player
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Destroy(gameObject);
            }

        }
    }

}
