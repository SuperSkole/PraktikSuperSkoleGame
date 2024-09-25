using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CorrectLetterHandler : MonoBehaviour
{
    private bool moving = false;
    private Vector3 startPos;
    private Vector3 endPos = new Vector3(-0.15f, 10.4f, -15.15f);
    private float startDistance;
    private float speed = 25;
    public TextMeshProUGUI exampleLetter;
    public RawImage image;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AutomaticClose());
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            float velocity = speed * Time.deltaTime;
            
            transform.position = Vector3.MoveTowards(transform.position, endPos, velocity);
            float newScale = Vector3.Distance(transform.position, endPos)/ startDistance;
            transform.localScale = new Vector3(newScale, newScale, newScale);

        }
    }

    private IEnumerator AutomaticClose()
    {
        yield return new WaitForSeconds(2);
        if(!moving)
        {
            StartMoving();
        }
    }

    private void StartMoving()
    {
        moving = true;
        startPos = transform.position;
        startDistance = Vector3.Distance(startPos, endPos);
    }

    public void OnClick()
    {
        StartMoving();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
