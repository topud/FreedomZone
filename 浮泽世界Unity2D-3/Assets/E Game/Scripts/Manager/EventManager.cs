using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;
using UnityEngine.EventSystems;

public class EventManager : SingletonClass<EventManager>
{
    public static EventSystem EventSystem { get => Singleton.GetComponent<EventSystem>(); }

    /// <summary>
    /// 获取鼠标停留处物体
    /// </summary>
    /// <param name="raycaster"></param>
    /// <returns></returns>
    public static GameObject GetOverGameObject()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        PhysicsRaycaster pr = CameraManager.Physics2DRaycaster;
        List<RaycastResult> results = new List<RaycastResult>();
        pr.Raycast(pointerEventData, results);
        if (results.Count != 0)
        {
            return results[0].gameObject;
        }
        return null;
    }
}