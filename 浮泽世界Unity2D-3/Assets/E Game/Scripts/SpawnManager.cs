using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class SpawnManager : SingletonPattern<SpawnManager>
{
    public GameObject HumanPrefab;
    [ReadOnly] public List<Character> Characters = new List<Character>();

    public Npc SpawnNpc(CharacterStaticData staticData, Vector2 position)
    {
        Transform parent = GameObject.Find("Characters").transform;
        GameObject target;
        if (staticData.Prefab)
        {
            target = Instantiate(staticData.Prefab, position, new Quaternion(0,0,0,0), parent);
            return target.GetComponent<Npc>();
        }
        else
        {
            target = Instantiate(HumanPrefab, position, new Quaternion(0, 0, 0, 0), parent);
            Npc character = target.AddComponent<Npc>();
            character.StaticData = staticData;
            character.ResetComponents();
            character.ResetDynamicData();
            return character;
        }
    }

    public Player SpawnPlayer(CharacterStaticData staticData, Vector2 position)
    {
        Transform parent = GameObject.Find("Characters").transform;
        GameObject target = Instantiate(HumanPrefab, position, new Quaternion(0, 0, 0, 0), parent);
        Player character = target.AddComponent<Player>();
        character.StaticData = staticData;
        character.ResetComponents();
        character.ResetDynamicData();
        return character;
    }
}
