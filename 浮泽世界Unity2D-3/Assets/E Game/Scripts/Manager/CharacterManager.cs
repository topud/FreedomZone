using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class CharacterManager : SingletonClass<CharacterManager>
{
    [Header("组件")]
    public GameObject HumanPrefab;

    [Header("数据")]
    [SerializeField, ReadOnly] private List<Character> characters = new List<Character>();
    public static List<Character> Characters
    {
        get
        {
            Singleton.characters.Clear();
            Character[] chars = Singleton.transform.GetComponentsInChildren<Character>();
            foreach (Character item in chars)
            {
                Singleton.characters.Add(item);
            }
            return Singleton.characters;
        }
    }

    private void OnEnable()
    {
        characters = Characters;
    }

    /// <summary>
    /// 生成角色，从静态数据
    /// </summary>
    /// <param name="sData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Character SpawnCharacter(string name, Vector2 position, bool isPlayer = false)
    {
        GameObject go;
        Character character;
        CharacterStaticData sData = (CharacterStaticData)CharacterStaticData.GetValue(name);
        if (sData)
        {
            if (sData.Prefab)
            {
                go = Instantiate(sData.Prefab, position, new Quaternion(0, 0, 0, 0), Singleton.transform);
                character = go.GetComponent<Character>();
                character.ResetDynamicData();
            }
            else
            {
                go = Instantiate(Singleton.HumanPrefab, position, new Quaternion(0, 0, 0, 0), Singleton.transform);
                character = go.GetComponent<Character>();
                character.StaticData = sData;
                character.ResetComponents();
                character.ResetDynamicData();
            }
            character.IsPlayer = isPlayer;
            Debug.Log("角色生成成功：" + name);
            return character;
        }
        else
        {
            Debug.LogError("静态数据不存在：" + name);
            return null;
        }
    }
    /// <summary>
    /// 生成角色，从动态数据（如存档）
    /// </summary>
    public static Character SpawnCharacter(CharacterDynamicData dData)
    {
        GameObject go;
        Character character;
        CharacterStaticData sData = (CharacterStaticData)CharacterStaticData.GetValue(dData.Name);
        if (sData)
        {
            if (sData.Prefab)
            {
                go = Instantiate(sData.Prefab, dData.Position, new Quaternion(0, 0, 0, 0), Singleton.transform);
                character = go.GetComponent<Character>();
                character.SetDynamicData(dData);
            }
            else
            {
                go = Instantiate(Singleton.HumanPrefab, dData.Position, new Quaternion(0, 0, 0, 0), Singleton.transform);
                character = go.GetComponent<Character>();
                character.StaticData = sData;
                character.ResetComponents();
                character.SetDynamicData(dData);
            }
            Debug.Log("角色生成成功：" + dData.Name);
            return character;
        }
        else
        {
            Debug.LogError("静态数据不存在：" + dData.Name);
            return null;
        }
    }
    /// <summary>
    /// 获取角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <returns></returns>
    public static Character GetCharacter(string name)
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
    public static Character GetCharacter(int id)
    {
        foreach (Character item in Characters)
        {
            if (item.gameObject.GetInstanceID() == id)
            {
                return item;
            }
        }
        return null;
    }
}
