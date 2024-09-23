using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmScript : MonoBehaviour
{
    [SerializeField]private AudioSource audioSource;
    [SerializeField]private AudioClip alarm;
    [SerializeField]private AudioClip policeSiren;
    private Quaternion startRotation;
    bool rotate = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            transform.Rotate(new Vector3(0, 0.25f, 0));
        }
    }

    public void DetermineAlarm(float mistakes)
    {
        if (mistakes > 0 && mistakes < 2)
        {
            audioSource.PlayOneShot(alarm);
        }
        if (mistakes > 1)
        {
            rotate = true;
            startRotation = transform.rotation;
        }
        if (mistakes >= 2)
        {
            audioSource.PlayOneShot(policeSiren);
        }
    }
}
