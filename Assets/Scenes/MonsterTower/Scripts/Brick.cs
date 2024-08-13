using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite sprite;
    public Sprite correctSprite;

    public bool checkCollision;
    public bool isShootable;
  


    // Update is called once per frame
    void Update()
    {
        if (checkCollision == true)
        {
            if (sprite == correctSprite)
            {
                gameObject.GetComponentInParent<TowerManager>().correctAnswer = true;
            }

            checkCollision = false;
        }
    }
}
