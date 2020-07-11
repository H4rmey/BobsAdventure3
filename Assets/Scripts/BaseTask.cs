using UnityEngine;
using System.Collections;

public class BaseTask : MonoBehaviour
{
    private GameObject  gameManager;
    private GridHandler grid;

	public bool			destinationIsSelf	= false;

    public Vector2      destination;
    public bool         isActive;      //override for the completion of the task

	private void Start()
	{
		if (destinationIsSelf)
		{
			destination = GetComponent<GridObject>().position;
		}
	}
}
