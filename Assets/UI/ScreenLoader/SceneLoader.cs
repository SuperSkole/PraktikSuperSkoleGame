using CORE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSingleton<SceneLoader>
{

    public GameObject loadingPreFab;

    private GameObject loadingScreenInstance;
    private AsyncOperation backgroundLoadOperation;

    private float CurrentProgress;

    private Image barfill;

    //Load here and now
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    //Load in background
    public void LoadSceneInBackground(string sceneName1, string sceneName2)
    {
        StartCoroutine(LoadSceneAsync(sceneName1));

        Debug.Log("second scene loading");
        backgroundLoadOperation = SceneManager.LoadSceneAsync(sceneName2, LoadSceneMode.Additive);
        backgroundLoadOperation.allowSceneActivation = false;
        backgroundLoadOperation.priority = -1;
        Debug.Log("second scene done");

    }

    //Load what has been loading in the background
    public void ActivateBackgroundLoadedScene()
    {
        Debug.Log("Background loading");
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
            loadingScreenInstance = Instantiate(loadingPreFab);

            // Find barfill in the instantiated prefab
            Transform parent = loadingScreenInstance.transform.Find("LoadingScreenPrefab");
            Transform barfillTransform = parent.transform.Find("BarFill");

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
        Debug.Log("Continue Loading");
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
        SetUpLoadingScreen();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float duration = 1f;
        float StartFillAmount = 0f;
        float elapsed = 0f;
        float oldFillAmount = 0f;

        CurrentProgress = operation.progress;

        //while its loading
        while (!operation.isDone)
        {
            //loading done
            if (barfill.fillAmount >= 0.9f && operation.progress >= 0.9f)
            {
                barfill.fillAmount = 1;
                operation.allowSceneActivation = true;
            }

            //if progress changes
            if(CurrentProgress != operation.progress)
            {
                //reset timer
                elapsed = 0f;
                CurrentProgress = operation.progress;
                StartFillAmount = oldFillAmount;

            }
            //if progress is the same
            if(CurrentProgress == operation.progress && elapsed < duration)
            {
                //count the time
                elapsed += Time.deltaTime;
                //calculate interpolation factor (0 to 1 over the duration)
                float t = Mathf.Clamp01(elapsed / duration);

                    if (barfill != null)
                    {
                        barfill.fillAmount = Mathf.Lerp(StartFillAmount, CurrentProgress, t);
                        oldFillAmount = barfill.fillAmount;
                    }

            }
            

            yield return null;
        }

        if (loadingScreenInstance != null)
        {
            Destroy(loadingScreenInstance);
        }
    }

    //private IEnumerator LoadSceneInBackgroundAsync(string sceneName1, string sceneName2)
    //{
    //    Debug.Log("starting loading first scene");

    //    yield return StartCoroutine(LoadSceneAsync(sceneName1));

    //    Debug.Log("first scene done loading");

    //    Resources.UnloadUnusedAssets();

    //    yield return new WaitForSeconds(3f);

    //    //Load in background
    //    Debug.Log("Doing second loading scene");
    //    backgroundLoadOperation = SceneManager.LoadSceneAsync(sceneName2, LoadSceneMode.Additive);
    //    backgroundLoadOperation.allowSceneActivation = false;
    //    backgroundLoadOperation.priority = -100;
    //    Debug.Log("set in function");      
     
    //}

}
