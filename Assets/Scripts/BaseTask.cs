﻿using UnityEngine;
using System.Collections;

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

    private void Awake()
	{
		if (destinationIsSelf)
		{
			destination = transform.position / GridHandler.cellSize;
		}
	}
}
