using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField]private List<Image> lifeImages;
    [SerializeField]private Sprite fullImage;
    [SerializeField]private Sprite halfImage;
    [SerializeField]private Sprite emptyImage;
    [SerializeField]private GameObject heartPrefab;
    
    public void SetHearts(int amount)
    {
        while (lifeImages.Count < amount)
        {
            GameObject newHeart = Instantiate(heartPrefab);
            newHeart.transform.SetParent(gameObject.transform);
            lifeImages.Add(newHeart.GetComponent<Image>());
        }
        while (lifeImages.Count > amount)
        {
            Destroy(lifeImages[0]);
            lifeImages.RemoveAt(0);
        }
        for(int i = 0; i < lifeImages.Count; i++)
        {
            if(lifeImages[i].sprite != fullImage)
            {
                lifeImages[i].sprite = fullImage;
            }
        }
    }

    public void ChangeHearts(float amount)
    {
        
    }
}
