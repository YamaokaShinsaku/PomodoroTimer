using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip notificationSound;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private TimeManager timeManager;

    // Start is called before the first frame update
    void Start()
    {
        timeManager.OnTimerComplete += OnTimerComplete;
    }

    void OnTimerComplete()
    {

    }

    public void PlayNotifivationSound()
    {
        if(audioSource && notificationSound)
        {
            audioSource.PlayOneShot(notificationSound);
        }
    }
}
