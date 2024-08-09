using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDeletor : MonoBehaviour
{
    [SerializeField] GameObject particals;
    BrickController targetbrick;
    /// <summary>
    /// when anything enters the trigger it spawns the particals and destroyes this gameobject
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(particals,transform.position,Quaternion.identity);
        targetbrick.checkCollision = true;
        Destroy(gameObject);
    }

    public void hitbox(BrickController brick)
    {
        targetbrick = brick;
    }
}
