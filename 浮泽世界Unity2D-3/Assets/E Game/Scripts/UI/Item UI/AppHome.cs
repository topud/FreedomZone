﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using E.Tool;
using E.Game;

public class AppHome : PhoneApp
{
    [Header("视图")]

    [Header("数据")]
    private Role target;
    public Role Target
    {
        get => target;
        set => target = value;
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}
