using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class MapManager : SingletonClass<AudioManager>
{
    public GameObject[] Maps
    {
        get
        {
            return GameObject.FindGameObjectsWithTag("Map");
        }
    }


    private void Start()
    {

    }
    private void Update()
    {
    }
}