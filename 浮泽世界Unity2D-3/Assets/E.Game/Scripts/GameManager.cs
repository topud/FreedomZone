using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Utility;

public class GameManager : SingletonPattern<GameManager>
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            SaveManager.Singleton.SaveGame();
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            SaveManager.Singleton.LoadGame();
        }
    }

}
