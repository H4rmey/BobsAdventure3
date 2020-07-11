using UnityEngine;
using System.Collections;

public class BaseTask
{
    public Vector2      origin;
    public Vector2      destination;
    public float        time;              //time to complete the task
    public bool         completedOverride;   //override for the completion of the task
}
