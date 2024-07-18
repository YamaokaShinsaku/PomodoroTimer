using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float workDuration = 25 * 60f;
    public float shortBreakDuration = 5 * 60f;
    public float longBreakDuration = 15 * 60f;
    public int sessionsBeforeLongBreak = 4;

    private float timer;
    private bool isRunning = false;
    private bool isWorkSession = true;
    private int sessionsCompleted = 0;

    private void Start()
    {
        InirializeTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                isRunning = false;
                //OnTimerComplete?.Invoke();

                if(isWorkSession)
                {
                    sessionsCompleted++;
                    if(sessionsCompleted % sessionsBeforeLongBreak == 0)
                    {
                        StartLongBreak();
                    }
                    else
                    {
                        StartShortBreak();
                    }
                }
                else
                {
                    StartWorkSession();
                }
            }
        }
    }

    public void InirializeTimer()
    {
        isWorkSession = true;
        timer = workDuration; // タイマーを作業時間に設定
    }

    public void StartWorkSession()
    {
        isWorkSession = true;
        timer = workDuration;
        isRunning = true;
    }

    public void StartShortBreak()
    {
        isWorkSession = false;
        timer = shortBreakDuration;
        isRunning = true;
    }

    public void StartLongBreak()
    {
        isWorkSession = false;
        timer = longBreakDuration;
        isRunning = true;
    }

    public void StartTimer()
    {
        isRunning = true;
        Debug.Log("Start");
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log("Stop");
    }

    public void ResetTimer()
    {
        isRunning = false;
        isWorkSession = true;
        workDuration = 25 * 60f;
        timer = workDuration;
        Debug.Log("Reset");
    }

    public float GetTimer()
    {
        return timer;
    }

    public void UpdateWorkDuration(float newWorkDuration)
    {
        workDuration = newWorkDuration * 60f;
        if(isWorkSession)
        {
            timer = workDuration;
        }
    }
}
