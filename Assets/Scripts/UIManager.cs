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

    public static UIManager instance;

    private void Start()
    {
        instance = this;
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
}
