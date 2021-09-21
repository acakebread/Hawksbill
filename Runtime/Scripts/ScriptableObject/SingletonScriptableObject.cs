// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:05:45 by seancooper
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : UnityEngine.ScriptableObject
{
    public static T Instance;
    public static T GetInstance() => Instance;

    protected virtual void OnEnable()
    {
        // if (!Application.isPlaying) return;
        Debug.Log (typeof (T) + "::OnEnable");
        if (Instance) Debug.LogWarning (typeof (T) + " not enabled " + this.GetInstanceID () + " already using instance: " + Instance.GetInstanceID ());
        else
        {
            Debug.Log (typeof (T) + " enabled " + this.GetInstanceID ());
            Instance = this as T;
        }
    }
}
