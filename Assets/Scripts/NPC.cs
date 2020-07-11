using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Vector2      position;

    public GridHandler  grid;

    public List<BaseTask>   tasksQueue;

    //sprites
    private     SpriteRenderer  spriteRenderer;
    public      Sprite[]        sprites;
    private     bool            spriteId    = false;
    private     bool            spriteFlip  = false;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<GridHandler>();

        GetRandomTasks();

        StartCoroutine("MoveToPosition", tasksQueue[0].destination);
    }

    private IEnumerator MoveToPosition(Vector2 aPosition)
    {
        while (true)
        {
            if (position.x != aPosition.x)
            {
                int directionX = harmClamp((int)aPosition.x - (int)position.x);

                spriteId = !spriteId;

                MoveNPC(new Vector2(directionX, 0));
            }
            if (position.y != aPosition.y)
            {
                int directionY = harmClamp((int)aPosition.y - (int)position.y);

                spriteFlip = !spriteFlip;

                MoveNPC(new Vector2(0, directionY));
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private bool MoveNPC(Vector2 aDirection)
    {
        if (grid.MoveCell((int)position.x, (int)position.y, (int)(position.x + aDirection.x), (int)(position.y + aDirection.y)))
        {
            position += aDirection;

            spriteRenderer          = grid.GetCell((int)position.x, (int)position.y).GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite   = sprites[Convert.ToInt32(spriteId)];
            spriteRenderer.flipX    = !spriteFlip;

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

    private void GetRandomTasks()
    {
        for (int i = 0; i < 3; i++)
        {
            int randomTaskId = UnityEngine.Random.Range(0, 4);

            BaseTask task = Resources.LoadAll<BaseTask>("Tasks")[randomTaskId];

            tasksQueue.Add(task);
        }
    }
}
