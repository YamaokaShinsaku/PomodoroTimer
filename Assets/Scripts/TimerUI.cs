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
    private const float MinWorkDuration = 1f;
    private const float StepDuration = 5f;
    private float currentSessionDuration; // 現在のセッション時間を管理する  

    // Start is called before the first frame update
    void Start()
    {
        currentSessionDuration = timeManager.workDuration;
        UpdateTimerText(timeManager.GetTimer());
        UpdateProgressBar(0.0f);
        notificationText.text = "作業時間：25 分";

        // セッションが変わった時の処理を追加
        timeManager.OnSessionChange += OnSessionChange;
    }

    void Update()
    {
        float currentTimer = timeManager.GetTimer();
        UpdateTimerText(currentTimer);

        // 作業時間に対して現在のタイマー値の割合を計算
        // プログレスバーを更新
        float progress = currentTimer / currentSessionDuration;
        if (timeManager.IsRunning())
        {
            UpdateProgressBar(progress);
        }
        //UpdateProgressBar(progress);
    }

    public void OnStartButtonClicked()
    {
        timeManager.StartTimer();
        UpdateNotificationText();
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
        currentSessionDuration = timeManager.workDuration;
        UpdateTimerText(timeManager.GetTimer());
        UpdateProgressBar(0.0f);
        notificationText.text = "作業時間 : 25分";
    }

    void UpdateTimerText(float time)
    {
        //int hours = Mathf.FloorToInt(time / 3600f);
        int minutes = Mathf.FloorToInt((time) / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        miniTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
        timeManager.UpdateWorkDuration((currentSessionDuration / 60) + StepDuration);
        Debug.Log("IncreaseWorkDuration: " + currentSessionDuration / 60 + StepDuration);
        currentSessionDuration = timeManager.GetCurrentSessionDuration();
        UpdateTimerText(currentSessionDuration);
        //UpdateProgressBar(1.0f);
        notificationText.text = $"作業時間 ：{currentSessionDuration / 60:f0} 分";
    }

    public void DecreaseWorkDuration()
    {
        float newDuration = (currentSessionDuration / 60) - StepDuration;
        if (newDuration < MinWorkDuration)
        {
            newDuration = MinWorkDuration;
        }
        timeManager.UpdateWorkDuration(newDuration);
        Debug.Log("DecreaseWorkDuration: " + newDuration);
        currentSessionDuration = timeManager.GetCurrentSessionDuration();
        UpdateTimerText(newDuration * 60);
        //UpdateProgressBar(1.0f);
        notificationText.text = $"作業時間 ：{newDuration:f0} 分";
    }

    // セッションが変わった時の処理
    private void OnSessionChange()
    {
        UpdateNotificationText();
        currentSessionDuration = timeManager.GetCurrentSessionDuration();
        Debug.Log("OnSessionChange: " + currentSessionDuration);
    }

    // 通知テキストを更新する
    private void UpdateNotificationText()
    {
        if (timeManager.IsWorkSession())
        {
            notificationText.text = "作業中";
            SoundManager.instance.PlaySound("作業開始");
        }
        else
        {
            notificationText.text = "休憩中";
            SoundManager.instance.PlaySound("休憩");
        }
    }
}
