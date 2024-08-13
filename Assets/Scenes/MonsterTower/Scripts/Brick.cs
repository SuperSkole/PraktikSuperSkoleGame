using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite sprite;
    public Sprite correctSprite;

    public bool checkCollision = false;
    public bool isShootable = false;
    public bool isCorrect = false;
  


    // Update is called once per frame
    void Update()
    {
        if (checkCollision && isCorrect)
        {
            gameObject.GetComponentInParent<TowerManager>().correctAnswer = true;
        }
    }
}
