using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseClick : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent leftClick;
    public UnityEvent middleClick;
    public UnityEvent rightClick;

    private void Start()
    {
        leftClick.AddListener(new UnityAction(MouseLeftClick));
        middleClick.AddListener(new UnityAction(MouseMiddleClick));
        rightClick.AddListener(new UnityAction(MouseRightClick));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            leftClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Middle)
            middleClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right)
            rightClick.Invoke();
    }

    private void MouseLeftClick()
    {
        Debug.Log("Mouse Left Click On " + name);
    }
    private void MouseMiddleClick()
    {
        Debug.Log("Mouse Middle Click On " + name);
    }
    private void MouseRightClick()
    {
        Debug.Log("Mouse Right Click On " + name);
    }
}
