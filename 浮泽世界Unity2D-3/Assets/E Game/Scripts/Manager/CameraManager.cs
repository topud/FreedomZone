﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using E.Tool;

public class CameraManager : SingletonClass<CameraManager>
{
    public Camera MainCamera;
    public Camera MinimapCamera;
    public CinemachineVirtualCamera VirtualCamera;

    public void SetFollow(Transform target)
    {
        VirtualCamera.Follow = target;
    }
    public void SetEdge(PolygonCollider2D target)
    {
        VirtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = target;
    }
    public void SetOrthographicSize(float value)
    {
        VirtualCamera.m_Lens.OrthographicSize = value;
    }
    public void ChangeOrthographicSize(float offset)
    {
        VirtualCamera.m_Lens.OrthographicSize += offset;
    }
}
