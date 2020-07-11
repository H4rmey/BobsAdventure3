using UnityEngine;

public class GridObject : MonoBehaviour
{
    [HideInInspector]
	public	Vector2			position;

    [HideInInspector]
    public  GridHandler     gridHandler;

    [HideInInspector]
    public  bool            hasLetter = false;

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

    public virtual GridObjectType GetGridType() { return GridObjectType.PROP; }
}

public enum GridObjectType
{
    NPC,
    PROP,
    PLAYER
}
