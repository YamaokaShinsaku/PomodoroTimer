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
                UIManager.instance.FadeOutTaskUIAnimation();
                UIManager.instance.ShowTaskMenuUI();
                break;
            case 2:
                UIManager.instance.CloseMainTimerUIEvent();
                break;
            case 3:
                TaskManager.instance.DeleteTask(this.gameObject);
                break;
            case 4:
                UIManager.instance.FadeInTaskUIAnimation();
                break;
            case 5:
                UIManager.instance.FadeOutTaskUIAnimation();
                break;
            case 6:
                CameraChanger.instance.changeCount++;
                break;
            case 7:
                break;
            default:
                Debug.Log("番号が範囲外です");
                break;
        }
    }
}
