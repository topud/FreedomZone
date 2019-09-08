using System.Collections;
using System.Collections.Generic;
using E.Tool;
using UnityEngine;

namespace E.Tool
{
    public class CharacterTrigger : MonoBehaviour
    {
        private Character character;

        private void Update()
        {
            if (!character)
            {
                character = transform.GetComponentInParent<Character>();
            }
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            //加入列表
            Character target = col.GetComponent<Character>();
            if (target)
            {
                if (!character.NearbyCharacters.Contains(target))
                {
                    character.NearbyCharacters.Add(target);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            //移出列表
            Character target = col.GetComponent<Character>();
            if (target)
            {
                character.NearbyCharacters.Remove(target);
            }

            //隐藏名称
            if (character.IsPlayer)
            {
                if (target)
                {
                    target.TargetUI.HideAll();
                    character.TargetUI.HideAll();
                }
            }
        }
    }
}