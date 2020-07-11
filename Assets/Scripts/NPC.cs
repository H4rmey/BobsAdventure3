using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Vector2 position;
    public GridHandler grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<GridHandler>();
        StartCoroutine("MoveToPosition", new Vector2(4, 8));
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator MoveToPosition(Vector2 aPosition)
    {
        while (true)
        {
            if (position.x != aPosition.x)
            {
                MoveNPC(new Vector2(harmClamp((int)aPosition.x - (int)position.x), 0));
            }
            if (position.y != aPosition.y)
            {
                MoveNPC(new Vector2(0, harmClamp((int)aPosition.y - (int)position.y)));
            }

            yield return new WaitForSeconds(0.25f);
        }
    }

    private bool MoveNPC(Vector2 aDirection)
    {
        if (grid.MoveCell((int)position.x, (int)position.y, (int)(position.x + aDirection.x), (int)(position.y + aDirection.y)))
        {
            position += aDirection;
            return true;
        }
        return false;
    }

    private int harmClamp(int value)
    {
        if (value < 0)
            return -1;
        if (value > 0)
            return 1;
        return 0;
    }
}
