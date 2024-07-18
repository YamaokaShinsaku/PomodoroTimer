using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int eventNumber = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Image clicked");
        switch(eventNumber)
        {
            case 1:
                UIManager.instance.OpenMainTimerUIEvene();
                break;
            case 2:
                UIManager.instance.CloseMainTimerUIEvent();
                break;
            case 3:
                break;
            default:
                Debug.Log("番号が範囲外です");
                break;
        }
    }
}
