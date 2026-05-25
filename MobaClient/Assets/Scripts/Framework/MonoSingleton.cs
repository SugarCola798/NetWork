using UnityEditor;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    
    private static bool isQuitting = false;
    
    private static T instance;

    public static T Instance
    {
        get
        {
            if (isQuitting)
                return null;
            if (instance == null)
            {
                instance  = FindObjectOfType<T>();
                if (instance == null)
                {
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        isQuitting = true;
        instance = null;
    }

    protected virtual void CleanInstance()
    {
        
    }
    
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod]
    private static void EditorInit()
    {
        EditorApplication.playModeStateChanged += state =>
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                instance = null;
                isQuitting = false;
            }
        };
    }
#endif
    
}
