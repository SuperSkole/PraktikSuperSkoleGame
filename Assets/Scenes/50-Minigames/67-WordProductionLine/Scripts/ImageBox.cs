using Assets.Scenes._50_Minigames._67_WordProductionLine.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBox : MonoBehaviour, IBox
{

    [SerializeField] private GameObject imageObject;

    public RawImage rawImage;

    public bool isSelected = false;


    void Start()
    {
        rawImage = imageObject.GetComponent<RawImage>();
    }

    /// <summary>
    /// Gets Image and displays on box
    /// </summary>
    /// <param name="textureImg"></param>
    public void GetImage(Texture2D textureImg)
    {
        rawImage.texture = textureImg;
    }


    /// <summary>
    /// resets the Cube, so the momentum dosnt stay.
    /// </summary>
    /// <param name="cube"></param>
    public void ResetCube()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        Rigidbody rb = gameObject.GetComponentInParent<Rigidbody>(true);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;


        rb.rotation = Quaternion.Euler(0, 0, 0);
        gameObject.transform.parent.rotation = Quaternion.Euler(0, 0, 0);
    }


}
