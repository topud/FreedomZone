using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using E.Tool;

public class CameraManager : SingletonClass<CameraManager>
{
    public Camera MainCamera;
    public Camera MinimapCamera;
    public CinemachineVirtualCamera VirtualCamera;

    public static Physics2DRaycaster Physics2DRaycaster
    { get => Singleton.GetComponentInChildren<Physics2DRaycaster>(); }

    public static void SetFollow(Transform target)
    {
        Singleton.VirtualCamera.Follow = target;
    }
    public static void SetEdge(PolygonCollider2D target)
    {
        Singleton.VirtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = target;
    }
    public static void SetOrthographicSize(float value)
    {
        Singleton.VirtualCamera.m_Lens.OrthographicSize = value;
    }
    public static void ChangeOrthographicSize(float offset)
    {
        Singleton.VirtualCamera.m_Lens.OrthographicSize += offset;
    }
}
