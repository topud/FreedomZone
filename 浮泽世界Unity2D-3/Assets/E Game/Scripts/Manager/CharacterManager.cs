using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;

public class CharacterManager : MonoBehaviour
{
    [Header("组件")]
    public GameObject HumanPrefab;

    [Header("数据")]
    [SerializeField] private Character player;
    [SerializeField, ReadOnly] private List<Character> characters = new List<Character>();

    public Character Player 
    {
        get
        {
            return player;
        }
        set 
        {
            if (player)
            {
                if (player != value)
                {
                    player.AIPath.enabled = false;
                }
            }
            if (value)
            {
                value.AIPath.enabled = true;
                GameManager.Camera.SetFollow(value.transform);
            }
            player = value;
        }
    }
    public List<Character> Characters
    {
        get
        {
            characters.Clear();
            Character[] chars = transform.GetComponentsInChildren<Character>();
            foreach (Character item in chars)
            {
                characters.Add(item);
            }
            return characters;
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
    public Character SpawnCharacter(string name, Vector2 position, bool isPlayer = false)
    {
        GameObject go;
        Character character;
        CharacterStaticData sData = (CharacterStaticData)CharacterStaticData.GetValue(name);
        if (sData)
        {
            if (sData.Prefab)
            {
                go = Instantiate(sData.Prefab, position, new Quaternion(0, 0, 0, 0), transform);
                character = go.GetComponent<Character>();
                character.ResetDynamicData();
            }
            else
            {
                go = Instantiate(HumanPrefab, position, new Quaternion(0, 0, 0, 0), transform);
                character = go.GetComponent<Character>();
                character.StaticData = sData;
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
    public Character SpawnCharacter(CharacterDynamicData dData)
    {
        GameObject go;
        Character character;
        CharacterStaticData sData = (CharacterStaticData)CharacterStaticData.GetValue(dData.Name);
        if (sData)
        {
            if (sData.Prefab)
            {
                go = Instantiate(sData.Prefab, dData.position, new Quaternion(0, 0, 0, 0), transform);
                character = go.GetComponent<Character>();
                character.SetDynamicData(dData);
            }
            else
            {
                go = Instantiate(HumanPrefab, dData.position, new Quaternion(0, 0, 0, 0), transform);
                character = go.GetComponent<Character>();
                character.StaticData = sData;
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
    public Character GetCharacter(int id)
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
