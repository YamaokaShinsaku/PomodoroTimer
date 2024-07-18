using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public Text timerText;
    public Text miniTimerText;
    public Text notificationText;
    public Image circleBar;
    [SerializeField]
    private TimeManager timeManager;

    // 作業時間の最小値（25分）および増減の単位（5分）
    private const float MinWorkDuration = 25f;
    private const float StepDuration = 5f;
    private float workDuration; // 現在の作業時間を管理する  

    // Start is called before the first frame update
    void Start()
    {
        workDuration = 25f;
        UpdateTimerText(workDuration * 60f);
        UpdateProgressBar(1.0f);
        notificationText.text = "作業時間：25 分";
    }

    void Update()
    {
        float currentTimer = timeManager.GetTimer();
        UpdateTimerText(currentTimer);

        // 作業時間に対して現在のタイマー値の割合を計算
        float progress = currentTimer / (workDuration * 60f);
        UpdateProgressBar(progress);
    }

    public void OnStartButtonClicked()
    {
        timeManager.StartTimer();
        if (timeManager.GetTimer() == timeManager.workDuration)
        {
            notificationText.text = "作業中";
        }
        else if (timeManager.GetTimer() == timeManager.shortBreakDuration 
            || timeManager.GetTimer() == timeManager.longBreakDuration)
        {
            notificationText.text = "休憩中";
        }
    }
    public void OnStopButtonClicked()
    {
        timeManager.StopTimer();
        notificationText.text = "停止中";
    }
    public void OnResetButtonClicked()
    {
        timeManager.ResetTimer();
        // リセット後、表示を更新
        workDuration = 25f;
        UpdateTimerText(timeManager.GetTimer());
        UpdateProgressBar(1.0f);
        notificationText.text = "作業時間 : 25分";
    }

    void UpdateTimerText(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600f);
        int minutes = Mathf.FloorToInt((time % 3600) / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        miniTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    public void UpdateProgressBar(float progress)
    {
        if (circleBar != null)
        {
            circleBar.fillAmount = progress;
        }
    }

    public void IncreaseWorkDuration()
    {       
        workDuration += StepDuration;
        timeManager.UpdateWorkDuration(workDuration);
        UpdateTimerText(workDuration * 60f);
        UpdateProgressBar(1.0f);
        notificationText.text = $"作業時間 : {workDuration:f0} 分";
    }

    public void DecreaseWorkDuration()
    {
        workDuration -= StepDuration;
        if (workDuration < MinWorkDuration)
        {
            workDuration = MinWorkDuration;
        }
        timeManager.UpdateWorkDuration(workDuration);
        UpdateTimerText(workDuration * 60f);
        UpdateProgressBar(1.0f);
        notificationText.text = $"作業時間 : {workDuration:f0} 分";
    }
}
