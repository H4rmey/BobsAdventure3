﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : GridObject
{
	public	int			cameraOffset	= -10;

	public	Camera		cam;

    public GameObject   gridObjectToInstantiate;

    private Text        interactionText;

    //sprites
    private     SpriteRenderer  spriteRenderer;
    public      Sprite[]        sprites;
    private     bool            spriteId = false;
    private     bool            spriteFlip = false;

    [Header("KeyBinds")]
	public	KeyCode		keyLeft				= KeyCode.LeftArrow;
	public	KeyCode		keyRight			= KeyCode.RightArrow;
	public	KeyCode		keyUp				= KeyCode.UpArrow;
	public	KeyCode		keyDown				= KeyCode.DownArrow;

	public	KeyCode		keyInteract			= KeyCode.E;
	
	private	GameObject	nearestInteractable;

	public override void Initialize()
	{
		base.Initialize();

		cam                 = Camera.main;
        interactionText     = GetComponentInChildren<Text>();
        type                = GridObjectType.PLAYER;
        hasLetter           = true;

        StartCoroutine("InputHandling");
		StartCoroutine("MoveCamera");
	}

	private IEnumerator InputHandling()
    {
        while (true)
        {
            if (Input.GetKeyDown(keyLeft))
            {
                spriteFlip = true;
                MovePlayer(new Vector2(-1, 0));
            }
			else if (Input.GetKeyDown(keyRight))
            {
                spriteFlip = false;
                MovePlayer(new Vector2(1, 0));
            }
			else if (Input.GetKeyDown(keyUp))
            {
                spriteFlip = !spriteFlip;
                MovePlayer(new Vector2(0, 1));
            }
			else if (Input.GetKeyDown(keyDown))
            {
                spriteFlip = !spriteFlip;
                MovePlayer(new Vector2(0, -1));
            }

			if (Input.GetKeyDown(keyInteract)) pInteract();

			yield return null;
		}
	}

	#region Movement

	private IEnumerator MoveCamera()
	{
		while (true)
		{
			cam.transform.position = new Vector3(position.x * GridHandler.cellSize, position.y * GridHandler.cellSize, cameraOffset);
			yield return null;
		}
	}

	private void MovePlayer(Vector2 aDirection)
	{
		if (gridHandler.MoveCell((int)position.x, (int)position.y, (int)(position.x + aDirection.x), (int)(position.y + aDirection.y)))
		{
			position += aDirection;

            spriteRenderer          = gridHandler.GetCell((int)position.x, (int)position.y).GetComponentInChildren<SpriteRenderer>();
            spriteId                = !spriteId;
            spriteRenderer.sprite   = sprites[Convert.ToInt32(spriteId)];
            spriteRenderer.flipX    = spriteFlip;
        }


        CheckInteraction(position + aDirection);
	}

	#endregion

	#region Interaction

	private void CheckInteraction(Vector2 aPosition)
	{
		if (gridHandler.CellIsOccupied((int)aPosition.x, (int)aPosition.y))
		{
			GameObject objectNextPlayer = gridHandler.GetCell((int)aPosition.x, (int)aPosition.y);
		
			if (objectNextPlayer.CompareTag("Interactable"))
			{
				nearestInteractable = objectNextPlayer;

                //Interaction Text
                if (hasLetter && objectNextPlayer.GetComponent<GridObject>().type == GridObjectType.NPC)
                {
                    interactionText.text = "Give Letter: <E>";
                }
                else
                {
                    interactionText.text = "Interact with " + objectNextPlayer.name + ": <E>"; //TODO: update name giving
                }

                return;
			}
        }
        else
        {
            interactionText.text = "";
        }

        nearestInteractable = default;
	}

	private void pInteract()
	{
		if (nearestInteractable == default) return;

		nearestInteractable.GetComponent<Interactable>().Interact(this);
    }

	#endregion
}
