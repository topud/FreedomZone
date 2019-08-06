using System.Collections;
using System.Collections.Generic;
using E.Utility;
using UnityEngine;

public class BodyTrigger : MonoBehaviour
{
    [ReadOnly, SerializeField] private Character character;
    public BodyPart BodyPart;

    private void Awake()
    {
        character = transform.parent.parent.GetComponent<Character>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}

public enum BodyPart
{
    头,
    体,
    手,
    脚
}