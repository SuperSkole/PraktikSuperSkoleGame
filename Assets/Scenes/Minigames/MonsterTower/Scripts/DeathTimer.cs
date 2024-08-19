using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    // Start is called before the first frame update

    public float targetTime = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
            TimerEnded();
        }

    }

    public void TimerEnded()
    {
        Destroy(gameObject);

    }

    
}
