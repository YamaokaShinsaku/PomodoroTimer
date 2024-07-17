using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public Text timerText;
    public Text notificationText;
    [SerializeField]
    private TimeManager timeManager;

    // 作業時間の最小値（25分）および増減の単位（5分）
    private const float MinWorkDuration = 25f;
    private const float StepDuration = 5f;
    private float workDuration; // 現在の作業時間を管理する

    // Start is called before the first frame update
    void Start()
    {
        // TimerManagerのイベントを購読
        timeManager.OnWorkSessionStart += OnWorkSessionStart;
        timeManager.OnShortBreakStart += OnShortBreakStart;
        timeManager.OnLongBreakStart += OnLongBreakStart;
        timeManager.OnTimerComplete += OnTimerComplete;

        workDuration = 25f;
        UpdateTimerText(workDuration * 60f);
    }

    void Update()
    {
        UpdateTimerText(timeManager.GetTimer());
    }

    void OnWorkSessionStart()
    {
        notificationText.text = "作業時間です！";
    }

    void OnShortBreakStart()
    {
        notificationText.text = "短い休憩時間です！";
    }

    void OnLongBreakStart()
    {
        notificationText.text = "長い休憩時間です！";
    }

    void OnTimerComplete()
    {
        notificationText.text = "タイマーが終了しました！";
    }

    void UpdateTimerText(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600f);
        int minutes = Mathf.FloorToInt((time % 3600) / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    public void IncreaseWorkDuration()
    {
        workDuration = timeManager.GetTimer() / 60f;
        workDuration += StepDuration;
        timeManager.UpdateWorkDuration(workDuration);
        UpdateTimerText(workDuration * 60f);
        notificationText.text = $"作業時間: {workDuration:f0} 分";
    }

    public void DecreaseWorkDuration()
    {
        workDuration = timeManager.GetTimer() / 60f;
        workDuration -= StepDuration;
        if (workDuration < MinWorkDuration)
        {
            workDuration = MinWorkDuration;
        }
        timeManager.UpdateWorkDuration(workDuration);
        UpdateTimerText(workDuration * 60f);
        notificationText.text = $"作業時間: {workDuration:f0} 分";
    }
}
