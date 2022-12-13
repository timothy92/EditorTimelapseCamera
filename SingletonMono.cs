/// <summary>
/// MIT License - Copyright(c) 2019 Ugo Belfiore
/// </summary>

using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// 
/// </summary>
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                if (Application.isPlaying)
                {
                    Debug.Log("is quitting ?");
                    return null;
                }
                else
                {
                    applicationIsQuitting = false;
                }
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogWarning("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");

                        return _instance;
                    }

                    if (_instance == null)
                    {

                    }
                    else
                    {

                    }
                }

                return _instance;
            }
        }
        set
        {
            _instance = value;
        }
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }
}