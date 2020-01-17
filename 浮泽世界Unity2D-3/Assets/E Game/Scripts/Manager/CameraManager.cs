using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using E.Tool;

public class CameraManager : MonoBehaviour
{
    public Camera main;
    public Camera minimap;
    public CinemachineVirtualCamera vc1;

    public Physics2DRaycaster Physics2DRaycaster
    { get => GetComponentInChildren<Physics2DRaycaster>(); }

    public void SetFollow(Transform target)
    {
        vc1.Follow = target;
    }
    public void SetEdge(PolygonCollider2D target)
    {
        vc1.GetComponent<CinemachineConfiner>().m_BoundingShape2D = target;
    }
    public void SetOrthographicSize(float value)
    {
        vc1.m_Lens.OrthographicSize = value;
    }
    public void ChangeOrthographicSize(float offset)
    {
        vc1.m_Lens.OrthographicSize += offset;
    }
}
