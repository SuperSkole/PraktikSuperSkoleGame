using UnityEngine;

public class GameTimer : MonoBehaviour
{

    //Script is not yet implemented xD

    public float TimeElapsed { get; private set; } = 0f;
    private bool isRunning = false;

    public void StartTimer()
    {
        isRunning = true;
        TimeElapsed = 0f;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void UpdateTimer(float deltaTime)
    {
        if (isRunning)
        {
            TimeElapsed += deltaTime;
        }
    }

    public string GetFormattedTime()
    {
        return string.Format("{0:00}:{1:00}.{2:000}", Mathf.FloorToInt(TimeElapsed / 60), Mathf.FloorToInt(TimeElapsed) % 60, Mathf.FloorToInt((TimeElapsed * 1000) % 1000));
    }
}
