using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class EntityManager : SingletonClass<EntityManager>
{
    public GameObject HumanPrefab;
    [ReadOnly] public List<Character> Characters = new List<Character>();
    [ReadOnly] public List<Interactor> Interactors = new List<Interactor>();

    /// <summary>
    /// 检查场景内的实体
    /// </summary>
    public void CheckSceneEntity()
    {
        Characters.Clear();
        Character[] characters = GameObject.Find("Characters").GetComponentsInChildren<Character>();
        foreach (Character item in characters)
        {
            Characters.Add(item);
        }
        Debug.Log("场景内NPC数量 " + Characters.Count);
        
        Interactors.Clear();
        Interactor[] interactors = GameObject.Find("Interactors").GetComponentsInChildren<Interactor>();
        foreach (Interactor item in interactors)
        {
            Interactors.Add(item);
        }
        Debug.Log("场景内物品数量 " + Interactors.Count);
    }

    /// <summary>
    /// 生成NPC
    /// </summary>
    /// <param name="sData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public Npc SpawnNpc(CharacterStaticData sData, Vector2 position)
    {
        Transform parent = GameObject.Find("Characters").transform;
        GameObject target;
        Npc npc;
        if (sData.Prefab)
        {
            target = Instantiate(sData.Prefab, position, new Quaternion(0, 0, 0, 0), parent);
            npc = target.GetComponent<Npc>();
        }
        else
        {
            target = Instantiate(HumanPrefab, position, new Quaternion(0, 0, 0, 0), parent);
            npc = target.AddComponent<Npc>();
            npc.StaticData = sData;
            npc.ResetComponents();
            npc.ResetDynamicData();
        }
        Characters.Add(npc);
        return npc;
    }
    /// <summary>
    /// 生成NPC
    /// </summary>
    public Npc SpawnNpc(CharacterDynamicData dData)
    {
        CharacterStaticData sData = (CharacterStaticData)CharacterStaticData.GetValue(dData.Name);
        return SpawnNpc(sData, dData.Position);
    }
    /// <summary>
    /// 生成玩家
    /// </summary>
    /// <param name="sData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public Player SpawnPlayer(CharacterStaticData sData, Vector2 position)
    {
        Transform parent = GameObject.Find("Characters").transform;
        GameObject target = Instantiate(HumanPrefab, position, new Quaternion(0, 0, 0, 0), parent);
        Player player = target.AddComponent<Player>();
        player.StaticData = sData;
        player.ResetComponents();
        player.ResetDynamicData();
        return player;
    }
    /// <summary>
    /// 生成玩家
    /// </summary>
    public Player SpawnPlayer(CharacterDynamicData dData)
    {
        CharacterStaticData sData = (CharacterStaticData)CharacterStaticData.GetValue(dData.Name);
        return SpawnPlayer(sData, dData.Position);
    }
    /// <summary>
    /// 生成Interactor
    /// </summary>
    /// <param name="sData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public Interactor SpawnInteractor(InteractorStaticData sData, Vector2 position)
    {
        Transform parent = GameObject.Find("Interactors").transform;
        GameObject target;
        Interactor inte;
        target = Instantiate(sData.Prefab, position, new Quaternion(0, 0, 0, 0), parent);
        inte = target.GetComponent<Interactor>();
        Interactors.Add(inte);
        return inte;
    }
    /// <summary>
    /// 生成Interactor
    /// </summary>
    public Interactor SpawnInteractor(InteractorDynamicData dData)
    {
        InteractorStaticData sData = (InteractorStaticData)InteractorStaticData.GetValue(dData.Name);
        return SpawnInteractor(sData, dData.Position);
    }

    /// <summary>
    /// 获取角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <returns></returns>
    public Character GetCharacter(string name)
    {
        foreach (Character item in Characters)
        {
            if (item.StaticData.Name == name)
            {
                return item;
            }
        }
        return null;
    }
    /// <summary>
    /// 获取物品
    /// </summary>
    /// <param name="name">物品名</param>
    /// <returns></returns>
    public Interactor GetInteractor(string name)
    {
        foreach (Interactor item in Interactors)
        {
            if (item.StaticData.Name == name)
            {
                return item;
            }
        }
        return null;
    }
}
