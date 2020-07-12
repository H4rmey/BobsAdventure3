using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : Interactable
{
	public BaseTask task;
    private TaskHandler taskHandler;

    public override void Initialize()
    {
        base.Initialize();

        task = GetComponent<BaseTask>();
        if (task != null)
        {
            taskHandler = GameObject.Find("Game Manager").GetComponent<TaskHandler>();
            taskHandler.taskList.Add(task);
        }
    }

    public override void Interact(GridObject aGridObject)
	{
		task.isActive = false;
        task.isTriggered = true;
        Debug.Log("interacted with: " + gameObject.name);
	}
}
