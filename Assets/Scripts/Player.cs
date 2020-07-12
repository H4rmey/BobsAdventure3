using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : GridObject
{
	public	int			cameraOffset	= -10;

	public	Camera		cam;

    public  GameObject  gridObjectToInstantiate;

    private Vector2     previousPos;
    private float       movementElapsedTime;
    public float        movementDelay;

    private Text        interactionText;
    private float       cameraElapsedTime;
    public float        cameraSmoothing;

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

	public	ItemObject	currentItem			= ItemObject.NONE;

	public override void Initialize()
	{
		base.Initialize();

		cam                 = Camera.main;
        interactionText     = GetComponentInChildren<Text>();
        hasLetter           = true;

        StartCoroutine("InputHandling");
		StartCoroutine("MoveCamera");
	}

    public override GridObjectType GetGridType() { return GridObjectType.PLAYER; }

    private IEnumerator InputHandling()
    {
        while (true)
        {
            while (movementElapsedTime >= 0 && movementElapsedTime < movementDelay)
            {
                movementElapsedTime += Time.deltaTime;
                yield return null;
            }

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
            Vector3 targetPos   = new Vector3(position.x * GridHandler.cellSize, position.y * GridHandler.cellSize, cameraOffset);
            Vector3 currentPos  = new Vector3(previousPos.x * GridHandler.cellSize, previousPos.y * GridHandler.cellSize, cameraOffset);

            while (cameraElapsedTime < cameraSmoothing)
            {
                cameraElapsedTime += Time.deltaTime;                
                
                cam.transform.position = Vector3.Slerp(currentPos, targetPos, cameraElapsedTime / cameraSmoothing);

                yield return null;
            }

            cam.transform.position = targetPos;
            yield return null;
        }
    }

	private void MovePlayer(Vector2 aDirection)
    {
        previousPos = position;

        if (cameraElapsedTime >= cameraSmoothing)
            cameraElapsedTime = 0;

        movementElapsedTime = 0;

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

                pInteractText(objectNextPlayer.GetComponent<GridObject>());

                return;
			}
        }
        
		ResetInteraction();
	}

	private void pInteract()
	{
		if (nearestInteractable == default) return;

		nearestInteractable.GetComponent<Interactable>().Interact(this);
		ResetInteraction();
    }

    private void pInteractText(GridObject aGridObject)
    {
        //Interaction Text
        if (hasLetter && aGridObject.GetGridType() == GridObjectType.NPC)
        {
            interactionText.text = "Give Letter: <E>";
        }
        else
        {
            interactionText.text = "Interact with " + aGridObject.objName + ": <E>"; //TODO: update name giving
        }
    }

	public void ResetInteraction()
	{
		interactionText.text = "";
        nearestInteractable = default;
	}

	#endregion

	public bool GiveItem(ItemObject aItem)
	{
		if (currentItem != ItemObject.NONE)
			DropCurrentItem();

		currentItem = aItem;

		return true;
	}

	private bool DropCurrentItem()
	{
		GameObject itemToDrop = GetItemObject();
		if (itemToDrop == null)
			return false;

		if (DropItem(position.x,		position.y - 1,	itemToDrop)	||
			DropItem(position.x + 1,	position.y,		itemToDrop)	||
			DropItem(position.x,		position.y + 1,	itemToDrop)	||
			DropItem(position.x - 1,	position.y,		itemToDrop)	)
			return true;

		return false;
	}

	private bool DropItem(float aX, float aY, GameObject aItemToDrop)
	{
		if (gridHandler.CellIsOccupied((int)aX, (int)aY)	||
			!gridHandler.InGridBounds((int)aX, (int)aY)		)
			return false;

		GameObject droppedItem = (GameObject)Instantiate(aItemToDrop, new Vector3(aX, aY, 0), Quaternion.identity);
		return true;
	}

	private GameObject GetItemObject()
	{
		switch(currentItem)
		{
			case ItemObject.FIRE_EXTINGUISHER:
				return (GameObject)Resources.Load("Fire_Extinguisher");
			case ItemObject.GOLDEN_PEN:
				return (GameObject)Resources.Load("Golden_Pen");

			default:
				return null;
		}
	}
}

public enum ItemObject
{
	NONE,
	FIRE_EXTINGUISHER,
	GOLDEN_PEN
}
