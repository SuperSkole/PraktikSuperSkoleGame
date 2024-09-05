using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Import.LeanTween.Framework;
using Scenes;

public class BalloonController : MonoBehaviour
{
    public bool isCorrect;
    public string letter;
    [SerializeField]private int targetX;
    [SerializeField]private float speed;

    /// <summary>
    /// sets a random x position that the balloon will move towards, then starts the animation to move it
    /// </summary>
    void Start()
    {
        targetX = Random.Range(0, 1800);
        speed = Random.Range(5f, 7f);
        //makes the button only work if the image is clicked
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;

        this.GetComponentInChildren<TMP_Text>().text = letter;
        LeanTween.move(gameObject, new Vector3(targetX, 1450, 0), speed).setOnComplete(DestroyMe);
    }


    /// <summary>
    /// destroys iteslf and removes a life if the answer was incorrect. If the player has no more lives, returns to the arcade
    /// </summary>
    void DestroyMe()
    {
        if(isCorrect)
        {
            BalloonSpawner mother = GetComponentInParent<BalloonSpawner>();
            mother.lives--;
            if (mother.lives <= 0)
            {
                SwitchScenes.SwitchToArcadeScene();
            }
        }
        if (!isCorrect)
        {

        }
        
        Destroy(gameObject);
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// called when the object attacthed is clicked on. Adds points if the balloon was correct and destroys itself.
    /// </summary>
    public void Clicked()
    {
        if(isCorrect)
        {
            BalloonSpawner mother = GetComponentInParent<BalloonSpawner>();
            mother.points++;
        }
        if(!isCorrect)
        {

        }
        Destroy(gameObject);
    }
}
