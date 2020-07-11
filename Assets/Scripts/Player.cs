using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	public	Vector2		position;

	public	int			cameraOffset	= -10;

	public	GridHandler	gridHandler;

	[Header("KeyBinds")]
	public	KeyCode	keyLeft				= KeyCode.LeftArrow;
	public	KeyCode	keyRight			= KeyCode.RightArrow;
	public	KeyCode	keyUp				= KeyCode.UpArrow;
	public	KeyCode	keyDown				= KeyCode.DownArrow;

	private void Start()
	{
		StartCoroutine("InputHandling");
		StartCoroutine("MoveCamera");
	}

	private IEnumerator InputHandling()
	{
		while(true)
		{
			if (Input.GetKeyDown(keyLeft)) MovePlayer(new Vector2(-1, 0));
			else if (Input.GetKeyDown(keyRight)) MovePlayer(new Vector2(1, 0));
			else if (Input.GetKeyDown(keyUp)) MovePlayer(new Vector2(0, 1));
			else if (Input.GetKeyDown(keyDown)) MovePlayer(new Vector2(0, -1));

			yield return null;
		}
	}

	private IEnumerator MoveCamera()
	{
		Camera cam = Camera.main;
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
	}
}
