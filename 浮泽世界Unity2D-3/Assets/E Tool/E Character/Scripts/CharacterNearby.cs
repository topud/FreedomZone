using System.Collections;
using System.Collections.Generic;
using E.Tool;
using UnityEngine;

namespace E.Tool
{
    public class CharacterNearby : MonoBehaviour
    {
        [ReadOnly, SerializeField] private Character character;

        private void Awake()
        {
            character = transform.parent.GetComponent<Character>();
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            Item item = col.GetComponent<Item>();
            if (item)
            {
                item.TargetUI.ShowName();
                if (!character.NearbyItems.Contains(item))
                {
                    character.NearbyItems.Add(item);
                }
            }

            Character target = col.GetComponent<Character>();
            if (target)
            {
                target.TargetUI.ShowName();
                if (!character.NearbyCharacters.Contains(target))
                {
                    character.NearbyCharacters.Add(target);
                }
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            Item item = col.GetComponent<Item>();
            if (item)
            {
                item.TargetUI.HideName();
                character.NearbyItems.Remove(item);
            }

            Character target = col.GetComponent<Character>();
            if (target)
            {
                target.TargetUI.HideName();
                character.NearbyCharacters.Remove(target);
            }
        }
    }
}