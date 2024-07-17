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

    // ��Ǝ��Ԃ̍ŏ��l�i25���j����ё����̒P�ʁi5���j
    private const float MinWorkDuration = 25f;
    private const float StepDuration = 5f;
    private float workDuration; // ���݂̍�Ǝ��Ԃ��Ǘ�����

    // Start is called before the first frame update
    void Start()
    {
        // TimerManager�̃C�x���g���w��
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
        notificationText.text = "��Ǝ��Ԃł��I";
    }

    void OnShortBreakStart()
    {
        notificationText.text = "�Z���x�e���Ԃł��I";
    }

    void OnLongBreakStart()
    {
        notificationText.text = "�����x�e���Ԃł��I";
    }

    void OnTimerComplete()
    {
        notificationText.text = "�^�C�}�[���I�����܂����I";
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
        notificationText.text = $"��Ǝ���: {workDuration:f0} ��";
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
        notificationText.text = $"��Ǝ���: {workDuration:f0} ��";
    }
}
