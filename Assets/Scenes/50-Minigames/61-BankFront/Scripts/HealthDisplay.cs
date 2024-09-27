using System.Collections;
using System.Collections.Generic;
using Scenes._50_Minigames._65_MonsterTower.Scrips;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField]private List<Image> lifeImages;
    [SerializeField]private Sprite fullImage;
    [SerializeField]private Sprite halfImage;
    [SerializeField]private Sprite emptyImage;
    [SerializeField]private GameObject heartPrefab;
    
    /// <summary>
    /// Sets the amount of hearts to the given amount and sets them all to full
    /// </summary>
    /// <param name="amount">The desired amount of hearts</param>
    public void SetHearts(int amount)
    {
        //Adds hearts until the amount of hearts match the amount
        while (lifeImages.Count < amount)
        {
            GameObject newHeart = Instantiate(heartPrefab);
            newHeart.transform.SetParent(gameObject.transform);
            lifeImages.Add(newHeart.GetComponent<Image>());
        }
        //removes hearts until the amount of them match the given amount
        while (lifeImages.Count > amount)
        {
            Destroy(lifeImages[0]);
            lifeImages.RemoveAt(0);
        }
        //Runs through all hearts and makes them into full hearts
        for(int i = 0; i < lifeImages.Count; i++)
        {
            if(lifeImages[i].sprite != fullImage)
            {
                lifeImages[i].sprite = fullImage;
            }
        }
    }

    /// <summary>
    /// Changes the amount of full and half hearts to empty or half hearts based on a given amount
    /// </summary>
    /// <param name="amount">the amount of hearts to make empty</param>
    public void ChangeHearts(float amount)
    {
        //Runs through the list of hearts and updates them if it can
        int index = 0;
        while(amount > 0 && index < lifeImages.Count)
        {
            //Changes a full heart to an empty one if the remaining amount is at or above 1
            if(amount >= 1 && lifeImages[index].sprite != emptyImage && lifeImages[index].sprite != halfImage)
            {
                amount--;
                lifeImages[index].sprite = emptyImage;
            }
            //Changes a full heart to a half one if atleast 0.5 remains of the amount
            else if(amount >= 0.5f && lifeImages[index].sprite == fullImage)
            {
                amount -= 0.5f;
                lifeImages[index].sprite = halfImage;
            }
            //Updates a half heart to an empty if there is atleast 0.5 remaining of the amount
            else if(amount >= 0.5f && lifeImages[index].sprite == halfImage)
            {
                amount -= 0.5f;
                lifeImages[index].sprite = emptyImage;
            }
            index++;
        }
    }
}
