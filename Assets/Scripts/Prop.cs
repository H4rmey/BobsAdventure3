using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : Interactable
{

	[Header("Is Item Zooi")]
	public	bool		isItem;
	public	ItemObject	itemItIs;

	[Header("Requires Item Zooi")]
	public	bool		requiresItem;
	public	ItemObject	itemItRequires;

	[Header("Task Zooi")]
	public	BaseTask	task;
	public	bool		isTask			= false;

    private TaskHandler taskHandler;

	private	Player		player;

    public override void Initialize()
    {
        base.Initialize();

		player = GameObject.Find("Player").GetComponent<Player>();

        task = GetComponent<BaseTask>();
        if (task != null)
        {
            taskHandler = GameObject.Find("Game Manager").GetComponent<TaskHandler>();

            if (!task.isFinalTask)
				taskHandler.taskList.Add(task);

			isTask		= true;
        }
    }

    public override void Interact(GridObject aGridObject)
	{
		if (requiresItem && player.currentItem != itemItRequires)
			return;

		if (isTask)
		{
			task.isActive		= false;
			task.isTriggered	= true;
		}

		if (isItem)
		{
			if (player.GiveItem(itemItIs))
			{
				Destroy(this.gameObject);
			}
		}

        Debug.Log("interacted with: " + gameObject.name);
	}
}
