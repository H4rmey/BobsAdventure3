using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : Interactable
{
	public	BaseTask	task;

	[Header("Is Item Zooi")]
	public	bool		isItem;
	public	ItemObject	itemItIs;

	[Header("Requires Item Zooi")]
	public	bool		requiresItem;
	public	ItemObject	itemItRequires;

    private TaskHandler taskHandler;

	private	Player		player;

    public  Sprite[]    iconSprite;

    public override void Initialize()
    {
        base.Initialize();

		player = GameObject.Find("Player").GetComponent<Player>();

        task = GetComponent<BaseTask>();
        if (task != null)
        {
            taskHandler = GameObject.Find("Game Manager").GetComponent<TaskHandler>();
            taskHandler.taskList.Add(task);
        }
    }

    public override void Interact(GridObject aGridObject)
	{
		if (requiresItem && player.currentItem != itemItRequires)
			return;

		task.isActive		= false;
        task.isTriggered	= true;

		if (isItem)
		{
			player.currentItem = itemItIs;
			Destroy(this.gameObject);
		}

        Debug.Log("interacted with: " + gameObject.name);
	}
}
