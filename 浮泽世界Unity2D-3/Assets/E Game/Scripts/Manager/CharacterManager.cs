using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E.Tool;
using UnityEngine.AddressableAssets;

public class CharacterManager : MonoBehaviour
{
    [Header("组件")]
    public GameObject HumanPrefab;

    [Header("数据")]
    [SerializeField] private Role player;
    [SerializeField, ReadOnly] private List<Role> characters = new List<Role>();

    public Role Player 
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
    public List<Role> Characters
    {
        get
        {
            characters.Clear();
            Role[] chars = transform.GetComponentsInChildren<Role>();
            characters.AddRange(chars);
            return characters;
        }
    }
    public int AvailableID
    {
        get
        {
            int max = short.MaxValue;
            int i = 0;
            do
            {
                bool isExsit = false;
                foreach (Role item in Characters)
                {
                    if (i == item.DynamicData.nameID.id)
                    {
                        isExsit = true;
                        break;
                    }
                }
                if (isExsit)
                {
                    i++;
                }
                else
                {
                    break;
                }
            } while (i < max);
            return i;
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
    public Role SpawnCharacter(string name, Vector2 position, bool isPlayer = false)
    {
        GameObject go;
        Role character;
        RoleStaticData sData = Addressables.LoadAsset<RoleStaticData>(name).Result;
        if (sData)
        {
            if (sData.prefab)
            {
                go = Instantiate(sData.prefab, position, new Quaternion(0, 0, 0, 0), transform);
                character = go.GetComponent<Role>();
                character.Refresh();
            }
            else
            {
                go = Instantiate(HumanPrefab, position, new Quaternion(0, 0, 0, 0), transform);
                character = go.GetComponent<Role>();
                character.SetStaticData(name);
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
    public Role SpawnCharacter(RoleDynamicData dData)
    {
        GameObject go;
        Role character;
        RoleStaticData sData = Addressables.LoadAsset<RoleStaticData>(dData.nameID).Result;
        if (sData)
        {
            if (sData.prefab)
            {
                go = Instantiate(sData.prefab, dData.position, new Quaternion(0, 0, 0, 0), transform);
                character = go.GetComponent<Role>();
                character.SetDynamicData(dData);
            }
            else
            {
                go = Instantiate(HumanPrefab, dData.position, new Quaternion(0, 0, 0, 0), transform);
                character = go.GetComponent<Role>();
                character.StaticData = sData;
                character.SetDynamicData(dData);
            }
            Debug.Log("角色生成成功：" + dData.nameID.NameID);
            return character;
        }
        else
        {
            Debug.LogError("静态数据不存在：" + dData.nameID.name);
            return null;
        }
    }
    /// <summary>
    /// 获取角色
    /// </summary>
    /// <param name="name">角色名</param>
    /// <returns></returns>
    public Role GetCharacter(NameAndID nameID)
    {
        foreach (Role item in Characters)
        {
            if (item.DynamicData.nameID.Equals(nameID))
            {
                return item;
            }
        }
        return null;
    }
}
