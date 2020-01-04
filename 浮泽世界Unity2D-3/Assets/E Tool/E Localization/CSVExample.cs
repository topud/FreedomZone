using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVExample : ScriptableObject
{
    public Sample[] m_Sample;
}

[Serializable]
public class Sample
{
    public int year;
    public string make;
    public string model;
    public string description;
    public float price;
}