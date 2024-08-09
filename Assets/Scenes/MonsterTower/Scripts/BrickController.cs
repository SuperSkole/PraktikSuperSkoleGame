using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite sprite;
    public Sprite correctSprite;

    [SerializeField] bool checkCollision;

    private void OnCollisionEnter(Collision collision)
    {
        if(sprite==correctSprite)
        {
            gameObject.GetComponentInParent<TowerManager>().correctAnswer = true;
        }
    }





    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (checkCollision == true)
        {
            if (sprite == correctSprite)
            {
                gameObject.GetComponentInParent<TowerManager>().correctAnswer= true;
            }
        }
    }
}
