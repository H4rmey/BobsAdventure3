using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NPC : Interactable
{
    public List<BaseTask>   tasksQueue;

    //sprites
    private     SpriteRenderer  spriteRenderer;
    public      Sprite[]        sprites;
    private     bool            spriteId    = false;
    private     bool            spriteFlip  = false;
    public      float           spriteSwapTime;

    private     float           timer             = 0;
    private     bool            doSpriteSwapping   = false;
    private     bool            taskSpriteId       = false;

    private     Image           taskIcon;
    private     Image           thinkCloudIcon;

    private		TaskHandler		taskHandler;

	private		int				currentTaskIndex;

	public override void Initialize()
	{
		base.Initialize();

        taskIcon = GameObject.Find("Canvas/Icon").GetComponent<Image>();
        thinkCloudIcon = GameObject.Find("Canvas/ThinkIcon").GetComponent<Image>();
        taskIcon.enabled = false;
        thinkCloudIcon.enabled = false;

        taskHandler = gridHandler.GetComponent<TaskHandler>();
	}

    public override GridObjectType GetGridType() { return GridObjectType.NPC; }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > spriteSwapTime && doSpriteSwapping)
        {
            timer = 0;

            taskIcon.enabled = true;
            thinkCloudIcon.enabled = true;

            taskIcon.sprite = tasksQueue[currentTaskIndex].iconSprite[Convert.ToInt32(taskSpriteId)];
            thinkCloudIcon.sprite = gridHandler.thinkCloudIcons[Convert.ToInt32(taskSpriteId)];

            taskSpriteId = !taskSpriteId;
        }
    }

    private IEnumerator TaskHandling()
    {
        GetRandomTasks();
        currentTaskIndex = 0;
        tasksQueue[currentTaskIndex].isTriggered = false;

        while (true)
        {
            doSpriteSwapping = true;
            Vector2 direction = tasksQueue[currentTaskIndex].destination - position;
			direction.x = HarmClamp((int)direction.x);
			direction.y = HarmClamp((int)direction.y);

            #region Movement
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
            #endregion

            if (ArrivedAtTask() && tasksQueue[currentTaskIndex].isTriggered)
            {
                //go to next task
                if (currentTaskIndex < tasksQueue.Count - 1)
                {
                    currentTaskIndex++;
                    tasksQueue[currentTaskIndex].isTriggered = false;
                }
				else
				{
                    //set target to bossman
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
            taskIcon.enabled        = false;
            thinkCloudIcon.enabled  = false;
            doSpriteSwapping        = false;

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
        for (int i = 0; i < 6; i++)
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

            doSpriteSwapping        = true;
            StartCoroutine("TaskHandling");
        }
    }
}
