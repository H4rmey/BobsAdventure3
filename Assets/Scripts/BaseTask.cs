using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Task", menuName = "Task/Task", order = 1)]
public class BaseTask : ScriptableObject
{
    private GameObject  gameManager;
    private GridHandler grid;

    public Vector2      destination;
    public bool         isActive;      //override for the completion of the task
}
