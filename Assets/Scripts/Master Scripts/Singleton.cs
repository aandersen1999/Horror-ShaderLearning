using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    var singleton = (GameObject) Resources.Load($"Static Prefabs/{typeof(T).Name}", typeof(GameObject));
                    Instantiate(singleton);
                    instance = singleton.GetComponent<T>();

                    /*GameObject obj = new()
                    {
                        name = typeof(T).Name
                    };
                    instance = obj.AddComponent<T>();*/
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static bool InstanceIsNull()
    {
        return instance == null;
    }
}
