using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SystemManager : Manager<SystemManager>
{
    public static UnityEvent<string> OnLanguageChanged = new UnityEvent<string>();
    private static Dictionary<string, List<string>> LanguageDictionary = new Dictionary<string, List<string>>();

    public static bool TryGetTranslation(string key, out string translateWord)
    {
        //TODO
        translateWord = key;
        return true;
    }

       
}
