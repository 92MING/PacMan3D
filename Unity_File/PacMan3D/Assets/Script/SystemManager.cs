using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SystemManager : Manager<SystemManager>
{
    public static UnityEvent<string> OnLanguageChanged = new UnityEvent<string>();
}
