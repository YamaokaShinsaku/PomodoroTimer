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

    // イベント
    public event Action OnWorkSessionStart;
    public event Action OnShortBreakStart;
    public event Action OnLongBreakStart;
    public event Action OnTimerComplete;

    private void Start()
    {
        timer = workDuration; // タイマーを作業時間に設定
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                isRunning = false;
                OnTimerComplete?.Invoke();

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

    public void StartWorkSession()
    {
        isWorkSession = true;
        timer = workDuration;
        isRunning = true;
        OnWorkSessionStart?.Invoke();
    }

    public void StartShortBreak()
    {
        isWorkSession = false;
        timer = shortBreakDuration;
        isRunning = true;
        OnShortBreakStart?.Invoke();
    }

    public void StartLongBreak()
    {
        isWorkSession = false;
        timer = longBreakDuration;
        isRunning = true;
        OnLongBreakStart?.Invoke();
    }

    public void StartTimer()
    {
        if(!isRunning)
        {
            StartWorkSession();
            isRunning = true;
            Debug.Log("Start");
        }
    }

    public void StopTimer()
    {
        isRunning = false;
        Debug.Log("Stop");
    }

    public void ResetTimer()
    {
        isRunning = false;
        timer = isWorkSession ? workDuration : shortBreakDuration;
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
