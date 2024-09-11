using CORE.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    [SerializeField] private float spawntimer = 1f;
    private Vector3 spawnpoint;
    string correctLetter;
    string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ";
    public int lives;
    public int points;
    TMP_Text text;
    [SerializeField] private GameObject topRight;
    [SerializeField] private GameObject topLeft;

    /// <summary>
    /// sets some values and starts the balloons
    /// </summary>
    void Start()
    {
        spawnpoint.z = 0;
        spawnpoint.y = -200;
        correctLetter += letters[Random.Range(0, letters.Length)];
        StartCoroutine(BalloonWave());
        text = GetComponentInChildren<TMP_Text>();
        
    }

    /// <summary>
    /// updates the text on the screen
    /// </summary>
    private void Update()
    {
        text.text = $"Klik på alle {correctLetter} \nDu har poppet {points} \nDu har {lives}/3 liv";
    }

    /// <summary>
    /// creates a new balloon at a random position below the screen and sets whether it is correct
    /// </summary>
    private void SpawnBalloon()
    {
        spawnpoint.x = Random.Range(topLeft.transform.position.x, topRight.transform.position.x-200);
        GameObject a = Instantiate(balloonPrefab, spawnpoint, this.transform.rotation, this.transform);
        BalloonController balloonSpawned = a.GetComponent<BalloonController>();
        int rnd = Random.Range(0, 2);
        if (rnd==1)
        {
            balloonSpawned.letter = correctLetter;
            balloonSpawned.isCorrect = true;
        }
        else
        {
            balloonSpawned.isCorrect = false;
            balloonSpawned.letter = "" + letters[Random.Range(0, letters.Length)];
            if(balloonSpawned.letter == correctLetter)
            {
                balloonSpawned.isCorrect = true;
            }
            
        }

        float target = Random.Range(topLeft.transform.position.x, topRight.transform.position.x-200);
        balloonSpawned.targetY = topRight.transform.position.y + 300;
        balloonSpawned.MoveTo(target);
    }

    /// <summary>
    /// creates a balloon after a short time, then sets a random timer before the next is spawned
    /// </summary>
    /// <returns></returns>
    IEnumerator BalloonWave()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawntimer);
            SpawnBalloon();
            spawntimer = Random.Range(0.5f, 2f);            
        }
    }

    
}
