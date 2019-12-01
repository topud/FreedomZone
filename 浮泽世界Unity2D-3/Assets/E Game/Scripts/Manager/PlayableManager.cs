using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;
using UnityEngine.Playables;

public class PlayableManager : SingletonClass<PlayableManager>
{
    public PlayableDirector[] Diectors
    {
        get => GetComponentsInChildren<PlayableDirector>();
    }

    private void Start()
    {

    }
    private void Update()
    {
    }
}