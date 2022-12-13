Credit to Ugo Belfiore for this script, I simplified this basically https://www.belfiore.ovh/devlogs/time-editor/time-editor.php

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class EditorTime : SingletonMono<EditorTime>
{

    private static float _editModeLastUpdate;
    private static float _timeScale = 1f;

    private static float _currentTime;          //current time timeScale-dependent (like Time.time but in editMode)
    private static float _currentTimeIndependentTimeScale;  //current time timeScale-independent (like Time.unscaledTime but in editMode)
    private static float _currentDeltaTime;     //current deltaTime timeScale-dependent (like Time.deltaTime but in editMode)
    private static float _currentUnscaledDeltaTime; //current deltaTime timeScale-independent (like Time.unscaledDeltaTime but in editMode)

    //don't let the deltaTime get higher than 1/30: if you have low fps (bellow 30),
    //the game start to go in slow motion.
    //if you don't want this behavior, put the value at 0.4 for exemple as precaution, we don't
    //want the player, after a huge spike of 5 seconds, to travel walls !
    private readonly float _maxSizeDeltaTime = 0.033f;

    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.update += EditorUpdate;
#endif
        _editModeLastUpdate = Time.realtimeSinceStartup;
        StartCoolDown();
    }

    protected void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= EditorUpdate;
#endif
    }


    /// <summary>
    /// at start, we initialize the current time
    /// </summary>
    private void StartCoolDown()
    {
        _currentTime = 0;
        _currentTimeIndependentTimeScale = 0;
        _editModeLastUpdate = Time.realtimeSinceStartup;
    }

#if UNITY_EDITOR
    private void EditorUpdate()
    {
        if (!Application.isPlaying)
        {
            EditorApplication.QueuePlayerLoopUpdate();
        }
    }
#endif

    /// <summary>
    /// called every frame in play and in editor, thanks to EditorApplication.QueuePlayerLoopUpdate();
    /// add to the current time, then save the current time for later.
    /// </summary>
    private void Update()
    {
        AddToTime();
        _editModeLastUpdate = Time.realtimeSinceStartup;
    }

    /// <summary>
    /// called every frame, add delta time to the current timer, with or not timeScale
    /// </summary>
    private void AddToTime()
    {
        _currentDeltaTime = (Time.realtimeSinceStartup - _editModeLastUpdate) * _timeScale;
        _currentDeltaTime = Mathf.Min(_currentDeltaTime, _maxSizeDeltaTime);    //if fps drop bellow 30fps, go into slow motion

        _currentUnscaledDeltaTime = (Time.realtimeSinceStartup - _editModeLastUpdate);
        _currentTime += _currentDeltaTime;
        _currentTimeIndependentTimeScale += _currentUnscaledDeltaTime;

        //SetSmoothDeltaTimes();
    }

    /// <summary>
    /// get the current timeScale
    /// </summary>
    public static float timeScale
    {
        get
        {
            return (_timeScale);
        }
        set
        {
            if (value != _timeScale)
            {
                _timeScale = value;
                Time.timeScale = Mathf.Max(0, _timeScale);
            }
        }
    }
    /// <summary>
    /// the time (timeScale dependent) at the begening of this frame (Read only). This is the time in seconds since the start of the game / the editor compilation
    /// </summary>
    public static float time
    {
        get
        {
            return (_currentTime);
        }
    }

    /// <summary>
    /// The timeScale-independant time for this frame (Read Only). This is the time in seconds since the start of the game / the editor compilation
    /// </summary>
    public static float unscaledTime
    {
        get
        {
            return (_currentTimeIndependentTimeScale);
        }
    }

    /// <summary>
    /// The completion time in seconds since the last from (Read Only)
    /// </summary>
    public static float deltaTime
    {
        get
        {
            return (_currentDeltaTime);
        }
    }

    /// <summary>
    /// The timeScale-independent interval in seconds from the last frames to the curren tone
    /// </summary>
    public static float unscaledDeltaTime
    {
        get
        {
            return (_currentUnscaledDeltaTime);
        }
    }
}
