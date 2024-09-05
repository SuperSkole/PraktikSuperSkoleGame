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

    void Start()
    {
        targetX = Random.Range(0, 1800);
        speed = Random.Range(5f, 7f);
        //makes the button only work if the image is clicked
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;

        this.GetComponentInChildren<TMP_Text>().text = letter;
        LeanTween.move(gameObject, new Vector3(targetX, 1450, 0), speed).setOnComplete(DestroyMe);
    }

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
