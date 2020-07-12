using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPC : Interactable
{
    public List<BaseTask>   tasksQueue;

    //sprites
    private     SpriteRenderer  spriteRenderer;
    public      Sprite[]        sprites;
    private     bool            spriteId    = false;
    private     bool            spriteFlip  = false;

	private		TaskHandler		taskHandler;

	private		int				currentTaskIndex;

	public override void Initialize()
	{
		base.Initialize();

		taskHandler = gridHandler.GetComponent<TaskHandler>();

		GetRandomTasks();
	}

    public override GridObjectType GetGridType() { return GridObjectType.NPC; }

    private IEnumerator TaskHandling()
    {
		currentTaskIndex = 0;

        while (true)
        {
			Vector2 direction = tasksQueue[currentTaskIndex].destination - position;
			direction.x = HarmClamp((int)direction.x);
			direction.y = HarmClamp((int)direction.y);


			//if (!ArrivedAtTask())
			//{
				
			//	else 
			//}
			//else
			//{
			//	if (currentTaskIndex < tasksQueue.Count)
			//	{
			//		currentTaskIndex++;
			//	}
			//	else
			//	{
			//		Debug.Log(this.gameObject.name + " Deded");
			//	}
			//	}

			if (position.x != tasksQueue[currentTaskIndex].destination.x)
			{
				if (!gridHandler.CellIsOccupied((int)(position.x + direction.x), (int)position.y))
				{
					if (MoveNPC(new Vector2(direction.x, 0)))
					{
						spriteId = !spriteId;
					}
				}
				else
				{
					if (MoveNPC(new Vector2(0, direction.y)))
					{
						spriteFlip = !spriteFlip;
					}
					else if (MoveNPC(new Vector2(0, -direction.y)))
					{
						spriteFlip = !spriteFlip;
					}
				}
			}

			else if (position.y != tasksQueue[currentTaskIndex].destination.y)
			{
				if (!gridHandler.CellIsOccupied((int)position.x, (int)(position.y + direction.y)))
				{
					if (MoveNPC(new Vector2(0, direction.y)))
					{
						spriteFlip = !spriteFlip;
					}
				}
				else 
				{
					if (MoveNPC(new Vector2(direction.x, 0)))
					{
						spriteId = !spriteId;
					}
					else if (MoveNPC(new Vector2(-direction.x, 0)))
					{
						spriteId = !spriteId;
					}
				}
			}

			if (ArrivedAtTask() && tasksQueue[currentTaskIndex].isActive)
			{
				if (currentTaskIndex < tasksQueue.Count - 1)
				{
					currentTaskIndex++;
				}
				else
				{
					Debug.Log("Done");
				}
			}

            yield return new WaitForSeconds(0.5f);
        }
    }

	private bool ArrivedAtTask()
	{
		if (position.x >= tasksQueue[currentTaskIndex].destination.x - 1	&&
			position.x <= tasksQueue[currentTaskIndex].destination.x + 1	&&
			position.y >= tasksQueue[currentTaskIndex].destination.y - 1	&&
			position.y <= tasksQueue[currentTaskIndex].destination.y + 1)
		{
			Debug.Log(this.gameObject.name + " dided a tasked");
			return true;
		}

		return false;
	}

    private bool MoveNPC(Vector2 aDirection)
    {
        if (gridHandler.MoveCell((int)position.x, (int)position.y, (int)(position.x + aDirection.x), (int)(position.y + aDirection.y)))
        {
            position += aDirection;

            spriteRenderer          = gridHandler.GetCell((int)position.x, (int)position.y).GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite   = sprites[Convert.ToInt32(spriteId)];
            spriteRenderer.flipX    = !spriteFlip;

            return true;
        }
        return false;
    }

    private int HarmClamp(int value)
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
            tasksQueue.Add(taskHandler.GetRandomTask());
        }
    }

    public override void Interact(GridObject aGridObject)
    {
        if (!hasLetter && aGridObject.hasLetter)
        {
            hasLetter               = true;
            aGridObject.hasLetter   = false;
            StartCoroutine("TaskHandling");
        }
    }
}
