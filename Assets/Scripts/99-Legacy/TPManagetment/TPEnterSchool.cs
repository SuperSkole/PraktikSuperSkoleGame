using UnityEngine;

namespace _99_Legacy.TPManagetment
{
    public class TPEnterSchool : MonoBehaviour
    {
        public GameObject tpPlacement;

        public void OnTriggerEnter(Collider collision)
        {
            Debug.Log("TP");
            if (collision.gameObject.tag == "Player")
            {
                collision.transform.position = tpPlacement.transform.position;
            }
        }
    }
}
