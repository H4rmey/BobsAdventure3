using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Vector2      position;

    public GridHandler  grid;

    public List<BaseTask>   tasksQueue;

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
                MoveNPC(new Vector2(harmClamp((int)aPosition.x - (int)position.x), 0));
            }
            if (position.y != aPosition.y)
            {
                MoveNPC(new Vector2(0, harmClamp((int)aPosition.y - (int)position.y)));
            }

            yield return new WaitForSeconds(0.5f);
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

    private void GetRandomTasks()
    {
        for (int i = 0; i < 3; i++)
        {
            int randomTaskId = Random.Range(0, 4);

            BaseTask task = Resources.LoadAll<BaseTask>("Tasks")[randomTaskId];

            tasksQueue.Add(task);
        }
    }
}
