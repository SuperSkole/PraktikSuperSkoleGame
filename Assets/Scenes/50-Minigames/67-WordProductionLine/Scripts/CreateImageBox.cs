using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateImageBox : MonoBehaviour
{
    [SerializeField]
    private GameObject topSpawnPoint;

    public bool isOn = true;

    [SerializeField]
    private ProductionLineObjectPool objectBoxPool;




    private void Start()
    {
        StartCoroutine(WaitForFourSeconds());
    }

    /// <summary>
    /// Creates ImageBoxes
    /// </summary>
    private void CreateProductionLineImageBox()
    {

        GameObject imageBox = objectBoxPool.GetPooledObject();

        if (imageBox != null)
        {
            imageBox.transform.position = topSpawnPoint.transform.position;
            imageBox.SetActive(true);
        }
    }

    /// <summary>
    /// waits 4 seconds...
    /// </summary>
    IEnumerator WaitForFourSeconds()
    {
        while (true)
        {


            // Wait for 4 seconds
            yield return new WaitForSeconds(4);

            if (isOn)
            {
                CreateProductionLineImageBox();
            }
        }
    }
}
