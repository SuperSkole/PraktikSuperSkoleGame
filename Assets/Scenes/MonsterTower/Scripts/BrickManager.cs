using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    // Start is called before the first frame update

    public string sentence;
    public Sprite correctImage;
    public List<Sprite> wrongImages;

    public BrickManager(string sentence,Sprite correctImage,List<Sprite>wrongImages)
    {
        this.sentence = sentence;
        this.correctImage = correctImage;
        this.wrongImages = wrongImages;


    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
