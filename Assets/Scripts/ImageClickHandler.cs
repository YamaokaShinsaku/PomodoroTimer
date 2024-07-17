using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int eventNumber = 0;
    [SerializeField]
    private GameObject miniTimerUI;
    [SerializeField]
    private GameObject mainTimerUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Image clicked");
        switch(eventNumber)
        {
            case 1:
                OpenMainTimerUIEvene();
                break;
            case 2:
                CloseMainTimerUIEvent();
                break;
            case 3:
                break;
            default:
                Debug.Log("番号が範囲外です");
                break;
        }
    }

    void OpenMainTimerUIEvene()
    {
        mainTimerUI.SetActive(true);
        miniTimerUI.SetActive(false);
    }

    void CloseMainTimerUIEvent()
    {
        mainTimerUI.SetActive(false);
        miniTimerUI.SetActive(true);
    }
}
