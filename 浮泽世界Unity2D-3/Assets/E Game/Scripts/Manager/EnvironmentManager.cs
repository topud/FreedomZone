﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using E.Tool;

public class EnvironmentManager : MonoBehaviour
{
    [Header("光线")]
    [SerializeField] private Gradient SunLightColor;
    [SerializeField, ReadOnly] private Light2D globleLight;

    public Light2D GlobleLight
    {
        get
        {
            if (!globleLight)
            {
                globleLight = GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light2D>();
            }
            return globleLight;
        }
    }

    [Header("粒子")]
    [SerializeField] private ParticleSystem Snow;
    [SerializeField] private ParticleSystem Rain;
    
    private void Update()
    {
        GlobleLight.color = SunLightColor.Evaluate(GameManager.Time.Current);
    }
}
