using System.Collections;
using System.Collections.Generic;
using E.Utility;
using UnityEngine;

public class NearByItemTrigger : MonoBehaviour
{
    [ReadOnly, SerializeField] private Character character;

    private void Awake()
    {
        character = transform.parent.GetComponent<Character>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        Item item = col.GetComponent<Item>();
        if (item != null)
        {
            item.CharacterUI.ShowName();
            if (!character.NearbyItems.Contains(item))
            {
                character.NearbyItems.Add(item);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        Item item = col.GetComponent<Item>();
        if (item != null)
        {
            item.CharacterUI.HideName();
            character.NearbyItems.Remove(item);
        }
    }
}
