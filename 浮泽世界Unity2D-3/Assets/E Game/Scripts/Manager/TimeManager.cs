using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using E.Tool;

public class TimeManager : SingletonClass<TimeManager>
{
    public bool Enable = false;
    public Gradient SunLightColor;
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

    public DateTime Now;
    [Range(1, 1440), Tooltip("现实中1秒游戏中？秒")] public float Ratio = 60;
    [Range(0.01f, 1), Tooltip("日光更新时间间隔")] public float Interval = 0.02f;
    [ReadOnly] public int Hour = 0;
    [ReadOnly] public int Minute = 0;
    [ReadOnly] public int Second = 0;
    [ReadOnly] public float Current = 0;
    private const int Max = 86400;

    private void Start()
    {
        Now = new DateTime(2020, 1, 1, 5, 0, 0);
    }
    private void OnEnable()
    {
        InvokeRepeating("UpdateTime", 0, Interval);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
    private void UpdateTime()
    {
        if (Enable)
        {
            Now = Now.AddSeconds(Interval * Ratio);
            Hour = Now.Hour;
            Minute = Now.Minute;
            Second = Now.Second;
            float current = Hour * 60 * 60 + Minute * 60 + Second;
            Current = current / Max;

            GlobleLight.color = SunLightColor.Evaluate(Current);
        }
    }
}
