using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorTimelapseTool : MonoBehaviour
{

    [SerializeField] float timelapseLengthMinutes = 0.1f;
    [SerializeField] Transform start, end;
    [SerializeField] AnimationCurve curve;

    float timelapseTime;
    float timelapseProgress;

    void Update()
    {
        if (start == null || end == null)
            return;

        if (timelapseTime >= (timelapseLengthMinutes * 60))
        {
            timelapseTime = 0;
        } 
        else
        {
            timelapseProgress = timelapseTime / (timelapseLengthMinutes * 60);
            timelapseTime += EditorTime.deltaTime;
        }


        transform.position = Vector3.Lerp(start.position, end.position, curve.Evaluate(timelapseProgress));
        transform.rotation = Quaternion.Slerp(start.rotation, end.rotation, curve.Evaluate(timelapseProgress));
    }

    [ContextMenu("Restart")]
    public void Restart()
    {
        timelapseTime = 0;
    }
}
