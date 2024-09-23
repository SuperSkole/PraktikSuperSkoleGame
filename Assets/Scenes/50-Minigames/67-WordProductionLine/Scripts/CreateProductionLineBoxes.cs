using System.Collections;
using UnityEngine;

public class CreateProductionLineBoxes : MonoBehaviour
{

    [SerializeField]
    private GameObject topSpawnPoint, botSpawnPoint;

    public bool isOn = true;


    private void FixedUpdate()
    {
        if (isOn)
        {
            CreateLetterBox();
        }
    }


    /// <summary>
    /// creates letterboxes.
    /// </summary>
    private void CreateLetterBox()
    {
        
        GameObject letterBox = ProductionLineObjectPool.instance.GetPooledObject();

        if (letterBox != null)
        {
            StartCoroutine(WaitForFourSeconds());
            letterBox.transform.position = botSpawnPoint.transform.position;
            letterBox.SetActive(true);
        }
    }

    /// <summary>
    /// waits 4 seconds...
    /// </summary>
    IEnumerator WaitForFourSeconds()
    {
        // Wait for 4 seconds
        yield return new WaitForSeconds(4);
    }
}
