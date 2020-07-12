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
    public      float           spriteSwapTime      = 0.5f;

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

	private		Vector2			previousPosition;

	public override void Initialize()
	{
		base.Initialize();

        taskIcon = GameObject.Find(gameObject.name + "/Canvas/Icon").GetComponent<Image>();
        thinkCloudIcon = GameObject.Find(gameObject.name + "/Canvas/ThinkIcon").GetComponent<Image>();
        taskIcon.enabled = false;
        thinkCloudIcon.enabled = false;

        taskHandler = gridHandler.GetComponent<TaskHandler>();
	}

    public override GridObjectType GetGridType() { return GridObjectType.NPC; }

    private void Update()
    {
        timer += Time.deltaTime;

        if (doSpriteSwapping && timer > spriteSwapTime)
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

		Vector2 direction = GetLargestDirection();

        while (true)
        {
        
			Vector2 generalDirection = currentTask.destination - position;
            doSpriteSwapping = true;

            if (ArrivedAtTask())
            {
				if (isDoingFinalTask)
				{
					gridHandler.gameEnds = true;
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
				
				Vector2 largestDir	= GetLargestDirection();
				Vector2 smallestDir	= GetSmallestDirection();

				if (MoveNPC(largestDir, true))
                {
                    direction = largestDir;
                    DoAnimate(direction);
                }
				else if (MoveNPC(smallestDir, true))
                {
                    direction = largestDir;
                    DoAnimate(direction);
                }

				else if (MoveNPC(direction, false))
				{
					DoAnimate(direction);
				}
				else if (MoveNPC(RotateDirection(direction), false))
				{
					generalDirection = RotateDirection(generalDirection);
					DoAnimate(direction);
				}
				else if (MoveNPC(RotateDirection(direction) * -1, false))
				{
					generalDirection = RotateDirection(generalDirection) * -1;
					DoAnimate(direction);
				}
				else
				{
					MoveNPC(direction * -1, false);
					generalDirection = generalDirection * -1;
				}

				#endregion
			}

            yield return new WaitForSeconds(0.5f);
        }
    }

	private void DoAnimate(Vector2 aDir)
	{
        if (aDir.x < 0)
        {
            spriteId = !spriteId;
            spriteFlip = true;
        }
        if (aDir.x > 0)
        {
            spriteId = !spriteId;
            spriteFlip = false;
        }
        if (aDir.y < 0)
        {
            spriteId = !spriteId;
            spriteFlip = !spriteFlip;
        }
        if (aDir.y > 0)
        {
            spriteId = !spriteId;
            spriteFlip = !spriteFlip;
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

    private bool MoveNPC(Vector2 aDirection, bool checkPrevPos)
    {
		if (checkPrevPos && previousPosition != null && position + aDirection == previousPosition)
			return false;

		Vector2	tempSavedPosition = position;

        if (gridHandler.MoveCell((int)position.x, (int)position.y, (int)(position.x + aDirection.x), (int)(position.y + aDirection.y)))
        {
			previousPosition = tempSavedPosition;

			position += aDirection;

            spriteRenderer          = gridHandler.GetCell((int)position.x, (int)position.y).GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite   = sprites[Convert.ToInt32(spriteId)];
            spriteRenderer.flipX    = !spriteFlip;

            return true;
        }
        return false;
    }

	private Vector2 GetLargestDirection()
	{
		Vector2 largest = currentTask.destination - position;

		if (Mathf.Abs(largest.x) > Mathf.Abs(largest.y))
		{
			return new Vector2(HarmClamp((int)largest.x), 0);
		}
		else
		{
			return new Vector2(0, HarmClamp((int)largest.y));
		}
	}

	private Vector2 GetSmallestDirection()
	{
		Vector2 largest = currentTask.destination - position;

		if (Mathf.Abs(largest.x) < Mathf.Abs(largest.y))
		{
			return new Vector2(HarmClamp((int)largest.x), 0);
		}
		else
		{
			return new Vector2(0, HarmClamp((int)largest.y));
		}
	}


	private Vector2 RotateDirection(Vector2 aDirection)
	{
		float tempX = aDirection.x;
		return new Vector2(aDirection.y, tempX);
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
