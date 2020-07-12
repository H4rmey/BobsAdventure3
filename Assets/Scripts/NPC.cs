using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NPC : Interactable
{
    //sprites
    private     SpriteRenderer  spriteRenderer;
    public      Sprite[]        sprites;
    private     bool            spriteId			= false;
    private     bool            spriteFlip			= false;
    public      float           spriteSwapTime;

	[Header("Task Settings")]
	public		BaseTask		firstTaskPref;
	public		BaseTask		secondTaskPref;
	[Range(0, 100)]
	public		float			firstPrefChance		= 30;
	[Range(0, 100)]
	public		float			secondPrefChance	= 20;
	public		BaseTask		currentTask;
	public		int				amountOfTaskToDo	= 5;

    private     float           timer				= 0;
    private     bool            doSpriteSwapping	= false;
    private     bool            taskSpriteId		= false;

    private     Image           taskIcon;
    private     Image           thinkCloudIcon;

    private		TaskHandler		taskHandler;

	private		int				amountTasksDone		= 0;

	private		bool			isDoingFinalTask	= false;

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

            taskIcon.sprite = currentTask.iconSprite[Convert.ToInt32(taskSpriteId)];
            thinkCloudIcon.sprite = gridHandler.thinkCloudIcons[Convert.ToInt32(taskSpriteId)];

            taskSpriteId = !taskSpriteId;
        }
    }

    private IEnumerator TaskHandling()
    {
        GetRandomTask();

        while (true)
        {
            doSpriteSwapping = true;
            Vector2 direction = currentTask.destination - position;
			direction.x = HarmClamp((int)direction.x);
			direction.y = HarmClamp((int)direction.y);

            if (ArrivedAtTask())
            {
				if (isDoingFinalTask)
				{
					///// END OF GAME
					Debug.Log("LETTER DELIVERED");
				}

				if (currentTask.isTriggered)
				{
					++amountTasksDone;

					//go to next task
					if (amountTasksDone < amountOfTaskToDo)
					{
						GetRandomTask();
					}
					else
					{
						currentTask = taskHandler.finalTask;
						currentTask.isTriggered = false;
						isDoingFinalTask = true;
					}
				}
            }
			else
			{
				#region Movement
				if (position.x != currentTask.destination.x)
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
						else if (MoveNPC(new Vector2(0, 1)))
						{
							spriteFlip = !spriteFlip;
						}
						else if (MoveNPC(new Vector2(0, -1)))
						{
							spriteFlip = !spriteFlip;
						}
					}
				}

				else if (position.y != currentTask.destination.y)
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
						else if (MoveNPC(new Vector2(1, 0)))
						{
							spriteId = !spriteId;
						}
						else if (MoveNPC(new Vector2(-1, 0)))
						{
							spriteId = !spriteId;
						}
					}
				}
				#endregion
			}

            yield return new WaitForSeconds(0.5f);
        }
    }

	private bool ArrivedAtTask()
	{
		if (CheckNextToTask())
        {
            taskIcon.enabled        = false;
            thinkCloudIcon.enabled  = false;
            doSpriteSwapping        = false;

            Debug.Log(this.gameObject.name + " dided a tasked");
			return true;
		}

		return false;
	}

	private bool CheckNextToTask()
	{
		if (position.x == currentTask.destination.x)
		{
			if (position.y == currentTask.destination.y - 1 ||
				position.y == currentTask.destination.y + 1)
			{
				return true;
			}
		}
		else if (position.y == currentTask.destination.y)
		{
			if (position.x == currentTask.destination.x - 1 ||
				position.x == currentTask.destination.x + 1)
			{
				return true;
			}
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

    private void GetRandomTask()
    {
		BaseTask newTask;

		do
		{
			float chance = UnityEngine.Random.Range(0, 100);

			if (chance < firstPrefChance)
				newTask = firstTaskPref;
			else if (chance < firstPrefChance + secondPrefChance)
				newTask = secondTaskPref;
			else
				newTask = taskHandler.GetRandomTask();

			if (currentTask == default)
				break;
		} while (newTask.destination == currentTask.destination);

		currentTask = newTask;
        currentTask.isTriggered = false;
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
