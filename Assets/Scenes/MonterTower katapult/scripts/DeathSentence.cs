using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSentence : MonoBehaviour
{
    [SerializeField] float AgeLimit;
    float age;

    /// <summary>
    /// counts up untill the agelimit and then destroyes this gameobject
    /// </summary>
    void Update()
    {
        if (age > AgeLimit)
        {
            Destroy(gameObject);
            return;
        }

        age += Time.deltaTime;
    }
}
