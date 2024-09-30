using CORE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    //private static SceneLoader instance;
    //public static SceneLoader Instance { get { return instance; } }

    public GameObject loadingPreFab;
    private GameObject loadingScreenInstance;
    private AsyncOperation backgroundLoadOperation;
    

    [SerializeField] Image barfill;

    //private void Awake()
    //{
        
    //    //Singleton pattern
    //    if (instance != null && instance != this)
    //    {
    //        Destroy(this.gameObject);

    //        return;
    //    }

    //        instance = this;
    //        DontDestroyOnLoad(this.gameObject);

    //}

    //Load here and now
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    //Load in background
    public void LoadSceneInBackground(string sceneName1, string sceneName2)
    {
        StartCoroutine(LoadSceneInBackgroundAsync(sceneName1, sceneName2));
    }

    //Load what has been loading in the background
    public void ActivateBackgroundLoadedScene()
    {
        
        if (backgroundLoadOperation != null)
        {
            if (backgroundLoadOperation.progress >= 0.9f)
            {
                backgroundLoadOperation.allowSceneActivation = true; 
            }
            else
            {
                Debug.LogWarning("The scene hasn't finished loading yet. Returning to loading screen.");
                StartCoroutine(ContinueLoading());
            }
        }
        else
        {
            Debug.LogWarning("No background scene has been loaded yet.");
        }
    }

    private void SetUpLoadingScreen()
    {
        // If the loading screen is already present, do not instantiate it again
        if (loadingScreenInstance == null && loadingPreFab != null)
        {
            Canvas canvas = FindAnyObjectByType<Canvas>();
            if (canvas != null)
            {
                loadingScreenInstance = Instantiate(loadingPreFab, canvas.transform);
            }
            else
            {
                loadingScreenInstance = Instantiate(loadingPreFab);
            }

            // Find barfill in the instantiated prefab
            Transform barfillTransform = loadingScreenInstance.transform.Find("BarFill");
            if (barfillTransform != null)
            {
                barfill = barfillTransform.GetComponent<Image>();
            }
            else
            {
                Debug.LogWarning("Barfill not found in the loading screen prefab.");
            }
        }
    }
    private IEnumerator ContinueLoading()
    {
        SetUpLoadingScreen();

        //if AsynOperation Backgroundload is not empty and is not done
        while (backgroundLoadOperation != null && !backgroundLoadOperation.isDone)
        {
            // Update the progress bar while continuing to load
            float progress = Mathf.Clamp01(backgroundLoadOperation.progress / 0.9f);
            if (barfill != null)
            {
                barfill.fillAmount = progress;
            }

            // If loading is done, activate the scene
            if (backgroundLoadOperation.progress >= 0.9f)
            {
                backgroundLoadOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Once loading is complete, destroy the loading screen
        if (loadingScreenInstance != null)
        {
            Destroy(loadingScreenInstance);
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.Log("Activated");

        SetUpLoadingScreen();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        //while its loading
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if(barfill != null)
            {
                barfill.fillAmount = progress;
            }

            //loading done
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (loadingScreenInstance != null)
        {
            Destroy(loadingScreenInstance);
        }
    }

    private IEnumerator LoadSceneInBackgroundAsync(string sceneName1, string sceneName2)
    {
        yield return StartCoroutine(LoadSceneAsync(sceneName1));

        //Load in background
        backgroundLoadOperation = SceneManager.LoadSceneAsync(sceneName2);
        backgroundLoadOperation.allowSceneActivation = false;

        // While the scene is loading in the background
        while (!backgroundLoadOperation.isDone)
        {
            float progress = Mathf.Clamp01(backgroundLoadOperation.progress / 0.9f);

            if (backgroundLoadOperation.progress >= 0.9f)
            {
                yield break;
            }

            yield return null;
        }
    }

}
