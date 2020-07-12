using UnityEngine;
using System.Collections;
using System;

public class BaseTask : MonoBehaviour
{
    private GameObject  gameManager;
    private GridHandler grid;

	public bool			destinationIsSelf	= false;

    public Vector2      destination;
    public bool         isActive			= false;
    public bool         isTriggered         = false;

	public bool			isFinalTask			= false;

    public              Sprite[]            iconSprite;

    public              Sprite[]            worldSprite;

    private void Awake()
	{
		if (destinationIsSelf)
		{
			destination = transform.position / GridHandler.cellSize;
		}
	}

    private void Update()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = worldSprite[Convert.ToInt32(isTriggered)];
    }
}
