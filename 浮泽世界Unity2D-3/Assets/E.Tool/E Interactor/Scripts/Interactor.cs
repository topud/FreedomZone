using System.Collections;
using System.Collections.Generic;
using E.Tool;
using E.Utility;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class Interactor : MonoBehaviour
{
    [Header("物品数据")]
    public InteractorStaticData StaticData;
    public InteractorDynamicData DynamicData;

    [Header("组件")]
    [ReadOnly, SerializeField] protected Animator Animator;
    [ReadOnly, SerializeField] protected AudioSource AudioSource;
    [ReadOnly, SerializeField] protected Rigidbody2D Rigidbody;
    [ReadOnly, SerializeField] protected Collider2D Collider;
    [ReadOnly, SerializeField] public CharacterUI CharacterUI;
    [ReadOnly, SerializeField] protected SortByDepth SortByDepth;


    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        CharacterUI = GetComponentInChildren<CharacterUI>();
        SortByDepth = GetComponent<SortByDepth>();
    }
    protected virtual void OnEnable()
    {
        CharacterUI.HideName();
        CharacterUI.HideTalk();
        CharacterUI.SetName(StaticData.Name);
        Rigidbody.mass = StaticData.Weight;
        gameObject.name = StaticData.Name;
    }
    protected virtual void Start()
    {
    }
    protected virtual void Update()
    {
    }
    protected virtual void OnDestroy()
    {

    }
    private void OnMouseEnter()
    {
        SortByDepth.SetColor(new Color(0.8f, 0.8f, 0.8f));
        if (UIManager.Singleton.entityInfoDisplayMode == UIManager.EntityInfoDisplayMode.HoverShowOnly ||
            UIManager.Singleton.entityInfoDisplayMode == UIManager.EntityInfoDisplayMode.HoverShowAndHitShow)
        {
            CharacterUI.ShowName();
        }
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1))
        {
            //UIManager.Singleton.uiCharacterInfo.SetCharacterEntity(this);
        }
    }
    private void OnMouseDown()
    {
        SortByDepth.SetColor(new Color(0.6f, 0.6f, 0.6f));
    }
    private void OnMouseDrag()
    {

    }
    private void OnMouseUp()
    {
        SortByDepth.SetColor(new Color(0.8f, 0.8f, 0.8f));
    }
    private void OnMouseExit()
    {
        SortByDepth.SetColor(new Color(1, 1, 1));
        if (UIManager.Singleton.entityInfoDisplayMode == UIManager.EntityInfoDisplayMode.HoverShowOnly ||
            UIManager.Singleton.entityInfoDisplayMode == UIManager.EntityInfoDisplayMode.HoverShowAndHitShow)
        {
            CharacterUI.HideName();
        }
    }
}
