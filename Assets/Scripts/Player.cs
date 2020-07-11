using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public	int			xPosition;
	public	int			yPosition;

	public	GridHandler	gridHandler;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if (gridHandler.MoveCell(xPosition, yPosition, xPosition - 1, yPosition))
			{
				--xPosition;
			}
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (gridHandler.MoveCell(xPosition, yPosition, xPosition + 1, yPosition))
			{
				++xPosition;
			}
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (gridHandler.MoveCell(xPosition, yPosition, xPosition, yPosition + 1))
			{
				++yPosition;
			}
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (gridHandler.MoveCell(xPosition, yPosition, xPosition, yPosition - 1))
			{
				--yPosition;
			}
		}
	}
}
