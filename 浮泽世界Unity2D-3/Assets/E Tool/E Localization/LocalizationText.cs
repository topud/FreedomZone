using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizationText : MonoBehaviour
{
    public string id;
    public UnityAction action;

    private void Start()
    {
        Refresh();

        action = new UnityAction(Refresh);
        LocalizationManager.OnLanguageChange.AddListener(action);
    }
    private void OnDestroy()
    {
        //if (LocalizationManager.OnLanguageChange != null)
        //{
        //    LocalizationManager.OnLanguageChange.RemoveListener(action);
        //}
    }

    private void Refresh()
    {
        if (!string.IsNullOrEmpty(id))
        {
            GetComponent<Text>().text = LocalizationManager.GetText(id);
        }
        else
        {
            Debug.LogError(name + " 未设置本地化id");
        }
    }
}
