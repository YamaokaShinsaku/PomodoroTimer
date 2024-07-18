using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject miniTimerUI;
    [SerializeField]
    private GameObject mainTimerUI;
    [SerializeField]
    private GameObject dateTimeUI;
    [SerializeField]
    private GameObject taskUI;
    [SerializeField]
    private GameObject OpenTaskMenuUI;
    [SerializeField]
    private GameObject CloseTaskMenuUI;

    public static UIManager instance;

    private void Start()
    {
        instance = this;
        FadeOutTaskUIAnimation();
    }

    public void OpenMainTimerUIEvene()
    {
        mainTimerUI.SetActive(true);
        //miniTimerUI.SetActive(false);
        FadeOutAnimation();
    }

    public void CloseMainTimerUIEvent()
    {
        mainTimerUI.SetActive(false);
        //miniTimerUI.SetActive(true);
        FadeInAnimation();
    }

    public void FadeOutAnimation()
    {
        miniTimerUI.transform.DOLocalMoveY(-470, 1.0f);
        dateTimeUI.transform.DOLocalMoveY(-470, 1.0f);
    }

    public void FadeInAnimation()
    {
        miniTimerUI.transform.DOLocalMoveY(-330, 1.0f);
        dateTimeUI.transform.DOLocalMoveY(-350, 1.0f);
    }

    public void FadeInTaskUIAnimation()
    {
        taskUI.transform.DOLocalMoveX(130, 1.0f);
        HideTaskManuUI();
    }

    public void FadeOutTaskUIAnimation()
    {
        taskUI.transform.DOLocalMoveX(360, 1.0f);
        ShowTaskMenuUI();
    }

    public void HideTaskManuUI()
    {
        OpenTaskMenuUI.SetActive(false);
        CloseTaskMenuUI.SetActive(true);
    }
    public void ShowTaskMenuUI()
    {
        OpenTaskMenuUI.SetActive(true);
        CloseTaskMenuUI.SetActive(false);
    }
}
