using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using E.Tool;
using UnityEngine.AddressableAssets;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField] private Language language = Language.CH;
    [NonSerialized] public UnityEvent OnLanguageChange = new UnityEvent();

    public List<Localizations> LocalizationsList
    {
        get
        {
            List<Localizations> list = new List<Localizations>();
            list.AddRange(Addressables.LoadAssets<Localizations>("Localizations", null).Result);
            return list;
        }
    }
    public Language Language
    {
        get => language;
        set
        {
            if (language != value)
            {
                language = value;
                OnLanguageChange.Invoke();
            }
        }
    }

    private void OnValidate()
    {
        OnLanguageChange.Invoke();
    }

    public string GetText(string id)
    {
        foreach (Localizations items in LocalizationsList)
        {
            foreach (Localization item in items.strings)
            {
                if (id == item.id)
                {
                    switch (Language)
                    {
                        case Language.CH:
                            return item.ch;
                        case Language.EN:
                            return item.en;
                        case Language.JP:
                            return item.jp;
                        default:
                            return item.ch;
                    }
                }
            }
        }
        return id;
    }
}

public enum Language
{
    CH,
    EN,
    JP
}