using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	public	Vector2		position;

	public	int			cameraOffset	= -10;

	public	GridHandler	gridHandler;

	public	Camera		cam;

    //sprites
    private     SpriteRenderer  sprite;
    public      Sprite[]        sprites;
    private     bool            spriteId;

    [Header("KeyBinds")]
	public	KeyCode		keyLeft				= KeyCode.LeftArrow;
	public	KeyCode		keyRight			= KeyCode.RightArrow;
	public	KeyCode		keyUp				= KeyCode.UpArrow;
	public	KeyCode		keyDown				= KeyCode.DownArrow;

	public	KeyCode		keyInteract			= KeyCode.E;
	
	private	GameObject	nearestInteractable;

	private void Start()
	{
		cam = Camera.main;

        sprite = gridHandler.GetCell((int)position.x, (int)position.y).GetComponentInChildren<SpriteRenderer>();

		StartCoroutine("InputHandling");
		StartCoroutine("MoveCamera");
	}

	private IEnumerator InputHandling()
	{
		while(true)
		{
			if (Input.GetKeyDown(keyLeft))
            {
                MovePlayer(new Vector2(-1, 0));

                sprite.sprite = sprites[0];
                spriteId = !spriteId;
            }
			else if (Input.GetKeyDown(keyRight))
            {
                MovePlayer(new Vector2(1, 0));

                sprite.sprite = sprites[0];
                spriteId = !spriteId;
            }
			else if (Input.GetKeyDown(keyUp))
            {
                MovePlayer(new Vector2(0, 1));

                sprite.sprite = sprites[1];
                spriteId = !spriteId;
            }
			else if (Input.GetKeyDown(keyDown))
            {
                MovePlayer(new Vector2(0, -1));

                sprite.sprite = sprites[1];
                spriteId = !spriteId;
            }

			if (Input.GetKeyDown(keyInteract)) Interact();

            Debug.Log(Convert.ToInt32(spriteId));

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
