using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSettingsUI : MonoBehaviour
{
    public Text timerText;
    [SerializeField]
    private TimeManager timeManager;

    // 作業時間の最小値、増減単位
    private const float MinWorkDuration = 25f;
    private const float StepDuration = 5f;
    // デフォルトの作業時間
    private float workDuration = 25f;

    // Start is called before the first frame update
    void Start()
    {
        // 作業時間を25分に初期設定する
        timeManager.UpdateWorkDuration(workDuration);
        UpdateWorkDurationText(workDuration * 60f);
    }

    private void UpdateWorkDurationText(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600f);
        int minutes = Mathf.FloorToInt((time % 3600) / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    public void IncreaseWorkDuration()
    {
        workDuration += StepDuration;
        timeManager.UpdateWorkDuration(workDuration);
        UpdateWorkDurationText(workDuration * 60f);
    }

    public void DecreaseWorkDuration()
    {
        workDuration -= StepDuration;
        if(workDuration < MinWorkDuration)
        {
            workDuration = MinWorkDuration;
        }
        timeManager.UpdateWorkDuration(workDuration);
        UpdateWorkDurationText(workDuration * 60f);
    }
}
