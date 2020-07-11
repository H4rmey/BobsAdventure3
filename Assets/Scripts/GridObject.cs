using UnityEngine;

public class GridObject : MonoBehaviour
{
	public	Vector2			position;

    public  GridHandler     gridHandler;

    public  GridObjectType  type;

    public  bool            hasLetter = false;

    private void Start()
	{
		Initialize();
        type = GridObjectType.PROP;
    }

	public virtual void Initialize()
	{
		gridHandler = GridHandler.Instance;
		position = new Vector2((int)transform.position.x / GridHandler.cellSize, (int)transform.position.y / GridHandler.cellSize);
        gridHandler.SetCell((int)position.x, (int)position.y, this.gameObject);
	}
}

public enum GridObjectType
{
    NPC,
    PROP,
    PLAYER
}
