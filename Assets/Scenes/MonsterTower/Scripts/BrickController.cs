using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Checks if the sprite the brick has is the correct sprite. 
// Start check by setting checkCollision=true.
public class BrickController : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite sprite;
    public Sprite correctSprite;

    [SerializeField] bool checkCollision;

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
