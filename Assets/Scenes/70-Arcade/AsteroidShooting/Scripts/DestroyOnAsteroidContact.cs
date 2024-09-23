using Scenes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DestroyOnAsteroidContact : MonoBehaviour
{
    [SerializeField] Vector3 spawnPoint;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] TextMeshProUGUI scoreTextMesh;
    [SerializeField] TextMeshProUGUI finalScoreTextMesh;
    

    public int lifePoints = 3;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// Using OnTriggerEnter2D to avoid physics collision with player lasers. 
    /// Is used for instantiating an explosion and either return the player to the start point 
    /// or show the final score and destroy the gameobject the component is attached to.
    /// Those two options are based on the amount of lifepoints left. 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.tag=="Asteroid")
        {
            Instantiate(explosionPrefab, gameObject.transform.position, transform.rotation, transform.parent);

            lifePoints -= 1;
            if (lifePoints > 0)
            {
                gameObject.transform.position = spawnPoint;
            }
            else
            {
                finalScoreTextMesh.gameObject.SetActive(true);
                finalScoreTextMesh.text = "Final" + scoreTextMesh.text;
                
                Destroy(gameObject);
            }


        }
    }


   
}
