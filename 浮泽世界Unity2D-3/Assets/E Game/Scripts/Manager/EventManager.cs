using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour
{
    public EventSystem EventSystem { get => FindObjectOfType<EventSystem>(); }

    /// <summary>
    /// 获取鼠标停留处物体
    /// </summary>
    /// <param name="raycaster"></param>
    /// <returns></returns>
    public GameObject GetOverGameObject()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        Physics2DRaycaster pr = GameManager.Camera.Physics2DRaycaster;
        List<RaycastResult> results = new List<RaycastResult>();
        pr.Raycast(pointerEventData, results);
        if (results.Count != 0)
        {
            return results[0].gameObject;
        }
        return null;
    }
}