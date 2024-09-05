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
    string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ∆ÿ≈";
    public int lives;
    public int points;
    TMP_Text text;

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
        text.text = $"Click all {correctLetter} \n You have clicked {points} \n you have {lives}/3 lives";
    }

    /// <summary>
    /// creates a new balloon at a random position below the screen and sets whether it is correct
    /// </summary>
    private void SpawnBalloon()
    {
        spawnpoint.x = Random.Range(0, 1800);
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
