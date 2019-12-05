using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using E.Tool;

public class LocalizationManager : SingletonClass<LocalizationManager>
{
    [SerializeField] private Language language = Language.CH;
    private static UnityEvent onLanguageChange = new UnityEvent();

    public static List<Localizations> LocalizationsList
    {
        get => Localizations.GetValues();
    }
    public static Language Language
    {
        get => Singleton.language;
        set
        {
            if (Singleton.language != value)
            {
                Singleton.language = value;
                OnLanguageChange.Invoke();
            }
        }
    }
    public static UnityEvent OnLanguageChange
    {
        get => onLanguageChange;
        set => onLanguageChange = value;
    }

    private void OnValidate()
    {
        OnLanguageChange.Invoke();
    }

    public static string GetText(string id)
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