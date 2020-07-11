using System;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
	public	static	int			amountCols	= 16;
	public	static	int			amountRows	= 16;
	public	static	int			cellSize	= 1;

	public			GridRow[]	grid		= new GridRow[amountRows];

	private void Start()
	{
		for (int x = 0; x < amountCols; x++)
		{
			for (int y = 0; y < amountRows; y++)
			{
				InitCell(x, y);
			}
		}
	}

	private void InitCell(int aXPos, int aYPos)
	{
		GameObject gridPrefab = grid[aYPos].row[aXPos];

		if (gridPrefab == default) return;

		GameObject gridObject			= Instantiate(gridPrefab);
		grid[aYPos].row[aXPos]			= gridObject;
		gridObject.transform.position	= new Vector2(aXPos * cellSize, aYPos * cellSize);
	}

	public GameObject GetCell(int aX, int aY)
	{
		if (!InGridBounds(aX, aY)) return null;

		return grid[aY].row[aX];
	}

	public void	SetCell(int aX, int aY, GameObject aObject)
	{
		if (!InGridBounds(aX, aY)) return;

		grid[aY].row[aX] = aObject;
		aObject.transform.position = new Vector2(aX * cellSize, aY * cellSize);
	}

	public void ResetCell(int aX, int aY)
	{
		grid[aY].row[aX] = default;
	}

	public bool MoveCell(int aOrigXPos, int aOrigYPos, int aNewXPos, int aNewYPos)
	{
		if (!InGridBounds(aNewXPos, aNewYPos)) return false;
		if (CellIsOccupied(aNewXPos, aNewYPos)) return false;

		GameObject obj = GetCell(aOrigXPos, aOrigYPos);
		SetCell(aNewXPos, aNewYPos, obj);
		ResetCell(aOrigXPos, aOrigYPos);

		return true;
	}

	public bool CellIsOccupied(int aX, int aY)
	{
		if (!InGridBounds(aX, aY))			return false;
		if (grid[aY].row[aX] != default)	return true;

		return false;
	}

	public bool InGridBounds(int aX, int aY)
	{
		if (aY >=	amountRows	||
			aX >=	amountCols	||
			aY <	0			||
			aX <	0)
		{
			return false;
		}

		return true;
	}
}

[System.Serializable]
public class GridRow
{
	public GameObject[] row = new GameObject[GridHandler.amountCols];
}
