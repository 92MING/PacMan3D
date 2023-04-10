using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Language
{
    zh_tw,
    zh_cn,
    en,
}

public class SystemManager : Manager<SystemManager>
{
    public static UnityEvent<Language> OnLanguageChanged = new UnityEvent<Language>(); //pass current language code
    private static Dictionary<string, string[]> LanguageDictionary = new Dictionary<string, string[]>(); //key, list of translated words
    public static Language _currentLanguage;
    public static Language currnetLanguage => _currentLanguage;

    public static bool TryGetTranslation(string key, out string translateWord, Language? lang = null)
    {
        var result = LanguageDictionary.TryGetValue(key, out string[] translateWords);
        if (result)
        {
            translateWord = translateWords[(int)(lang??currnetLanguage)];
        }
        else
        {
            translateWord = key;
        }
        return result;
    }

       
}
