using UnityEngine;
using System.Collections;

public class BaseTask : MonoBehaviour
{
    private GameObject  gameManager;
    private GridHandler grid;

	public bool			destinationIsSelf	= false;

    public Vector2      destination;
    public bool         isActive			= true;      //override for the completion of the task

	private void Awake()
	{
		if (destinationIsSelf)
		{
			destination = transform.position / GridHandler.cellSize;
		}
	}
}
