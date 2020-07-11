using UnityEngine;

public class GridObject : MonoBehaviour
{
	public	Vector2			position;

    public  GridHandler     gridHandler;

	private void Start()
	{
		Initialize();
	}

	public virtual void Initialize()
	{
		gridHandler = GridHandler.Instance;
		position = new Vector2((int)transform.position.x / GridHandler.cellSize, (int)transform.position.y / GridHandler.cellSize);
        gridHandler.SetCell((int)position.x, (int)position.y, this.gameObject);
	}
}
