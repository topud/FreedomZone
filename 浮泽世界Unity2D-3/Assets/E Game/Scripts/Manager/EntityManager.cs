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
    /// 生成角色，从静态数据
    /// </summary>
    /// <param name="sData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public Character SpawnCharacter(CharacterStaticData sData, Vector2 position, bool isPlayer = false)
    {
        Transform parent = GameObject.Find("Characters").transform;
        GameObject go;
        Character character;
        if (sData.Prefab)
        {
            go = Instantiate(sData.Prefab, position, new Quaternion(0, 0, 0, 0), parent);
            character = go.GetComponent<Character>();
        }
        else
        {
            go = Instantiate(HumanPrefab, position, new Quaternion(0, 0, 0, 0), parent);
            character = go.GetComponent<Character>();
            character.StaticData = sData;
            character.ResetComponents();
            character.ResetDynamicData();
        }
        character.IsPlayer = isPlayer;
        Characters.Add(character);
        return character;
    }
    /// <summary>
    /// 生成角色，从动态数据（如存档）
    /// </summary>
    public Character SpawnCharacter(CharacterDynamicData dData, bool isPlayer = false)
    {
        CharacterStaticData sData = (CharacterStaticData)CharacterStaticData.GetValue(dData.Name);
        Character character = SpawnCharacter(sData, dData.Position, isPlayer);
        return character;
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
