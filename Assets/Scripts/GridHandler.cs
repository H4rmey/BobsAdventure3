using System;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
	private readonly	int			amountCols	= 32;
	private	readonly	int			amountRows	= 32;
	private	readonly	int			cellSize	= 1;

	public				GridRow[]	grid		= new GridRow[32];



	public GridObject GetCell(int aY, int aX)
	{
		if (!InGridBounds(aY, aX)) return null;

		return grid[aY].row[aX];
	}

	public void	SetCell(int aY, int aX, GridObject aObject)
	{
		if (!InGridBounds(aY, aX)) return;

		grid[aY].row[aX] = aObject;
	}

	private bool InGridBounds(int aY, int aX)
	{
		if (aY >=	amountRows	||
			aX >=	amountCols	||
			aY <	0			||
			aX <	0)
		{
			Debug.LogError("GetCell: Out of bounds");
			return false;
		}

		return true;
	}
}

[System.Serializable]
public class GridRow
{
	public GridObject[] row = new GridObject[32];
}
