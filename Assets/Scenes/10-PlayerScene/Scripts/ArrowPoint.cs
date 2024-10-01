using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrowPoint : MonoBehaviour
{

    private Vector3 target;

    private void Start()
    {
        SceneManager.sceneLoaded += Setup;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= Setup;
    }

    void Setup(Scene scene, LoadSceneMode mode)
    {
        target = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
        gameObject.SetActive(true);
    }


    void Update()
    {
        transform.LookAt(target);
        transform.Rotate(new(90,0,0));
    }
}
