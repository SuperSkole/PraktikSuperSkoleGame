using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmScript : MonoBehaviour
{
    [SerializeField]private AudioClip alarm;
    [SerializeField]private AudioClip policeSiren;
    bool rotate = false;

    public bool playingSound = false;
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
        if (mistakes > 0 && mistakes <= 2)
        {
            AudioManager.Instance.PlaySound(alarm, SoundType.SFX, transform.position);
        }
        if (mistakes > 1)
        {
            rotate = true;
        }
        if (mistakes > 2)
        {
            AudioManager.Instance.PlaySound(policeSiren, SoundType.SFX, transform.position);
            StartCoroutine(PlayingSiren());
        }
    }

    private IEnumerator PlayingSiren()
    {
        playingSound = true;
        yield return new WaitForSeconds(policeSiren.length);
        playingSound = false;
    }
}