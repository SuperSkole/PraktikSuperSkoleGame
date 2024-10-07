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
    [SerializeField] PolygonCollider2D playerCollider;
    [SerializeField] SpriteRenderer playerSpriteRenderer;

    private bool isInvincible;
    

    public int lifePoints = 3;

    private float timer=0;

    private float blinkingTimer;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isInvincible)
        {
            timer += Time.deltaTime;

            if (Mathf.FloorToInt(timer)%2==0)
            {
                playerSpriteRenderer.color = Color.white;
            }
            else
            {
                playerSpriteRenderer.color = Color.gray;
            }
            

            if (timer >= 4)
            {
                timer = 0;
                playerCollider.enabled = true;
                isInvincible = false;
            }

            
        }

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
                isInvincible = true;

                playerCollider.enabled = false;
               
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
