using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using E.Tool;

public class PlayableManager : MonoBehaviour
{
    public PlayableDirector[] Directors
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