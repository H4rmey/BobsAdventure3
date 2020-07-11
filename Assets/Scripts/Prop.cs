using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : Interactable
{
	public BaseTask task;

	public override void Interact(GridObject aGridObject)
	{
		task.isActive = false;
	}
}
