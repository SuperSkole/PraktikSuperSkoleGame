using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesableOnLoade : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}
