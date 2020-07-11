using System;
using System.Collections;
using UnityEngine;

public class Player : GridObject
{
	public	int			cameraOffset	= -10;

	public	Camera		cam;

    public GameObject   gridObjectToInstantiate;

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

		cam = Camera.main;

        StartCoroutine("InputHandling");
		StartCoroutine("MoveCamera");
	}

	private IEnumerator InputHandling()
    {
        while (true)
        {
            if (Input.GetKeyDown(keyLeft))
            {
                MovePlayer(new Vector2(-1, 0));
                spriteFlip = true;
            }
			else if (Input.GetKeyDown(keyRight))
            {
                MovePlayer(new Vector2(1, 0));
                spriteFlip = false;
            }
			else if (Input.GetKeyDown(keyUp))
            {
                MovePlayer(new Vector2(0, 1));
                spriteFlip = !spriteFlip;
            }
			else if (Input.GetKeyDown(keyDown))
            {
                MovePlayer(new Vector2(0, -1));
                spriteFlip = !spriteFlip;
            }

			if (Input.GetKeyDown(keyInteract)) Interact();

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
            spriteRenderer.sprite   = sprites[Convert.ToInt32(spriteId)];
            spriteId                = !spriteId;
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
				return;
			}
		}

		nearestInteractable = default;
	}

	private void Interact()
	{
		if (nearestInteractable == default) return;

		nearestInteractable.GetComponent<Interactable>().Interact();
	}

	#endregion
}
