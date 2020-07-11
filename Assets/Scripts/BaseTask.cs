using UnityEngine;
using System.Collections;

public class BaseTask : MonoBehaviour
{
    private GameObject  gameManager;
    private GridHandler grid;

    public Vector2      destination;
    public bool         isActive;      //override for the completion of the task

    public void InitTask(Vector2 aPos)
    {
        gameManager = GameObject.Find("Game Manager");
        grid = gameManager.GetComponent<GridHandler>();
    }
}
