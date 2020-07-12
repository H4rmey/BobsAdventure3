using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskHandler : MonoBehaviour
{
    public	List<BaseTask>	taskList;
	public	BaseTask		finalTask;

	public BaseTask GetRandomTask()
	{
		int taskId = Random.Range(0, taskList.Count);
		return taskList[taskId];
	}
}
