using UnityEngine;
using System.Collections;

public class BaseTask : MonoBehaviour
{
    private GameObject  gameManager;
    private GridHandler grid;

    public Vector2      destination;
    public bool         isActive;      //override for the completion of the task
}
