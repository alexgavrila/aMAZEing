using UnityEngine;
using System.Collections.Generic;
using Random = System.Random;

public class MazeRoom : ScriptableObject
{
	public int settingsIndex;

	public MazeRoomSettings settings;

	private List<MazeCell> cells = new List<MazeCell>();

	public void Add(MazeCell cell)
	{
		cell.room = this;
		cells.Add(cell);
	}
	public void Assimilate(MazeRoom room)
	{
		for (int i = 0; i < room.cells.Count; i++)
		{
			Add(room.cells[i]);
		}
	}

	public List<MazeCell> GetRoomCells()
	{
		return cells;
	}

	public int RoomSize()
	{
		return cells.Count;
	}

	public MazeCell PickRandomCell()
	{
		return cells[UnityEngine.Random.Range(0, RoomSize())];
	}

	// Get a cell without walls or doors
	public MazeCell PickRandomEmptyCell()
	{
		int noOfTries = cells.Count * 3;
		
		MazeCell randomCell = null;
		for (var tries = 0; tries < noOfTries; tries++)
		{
			randomCell = PickRandomCell();

			if (randomCell.IsEmptyCell())
			{
				return randomCell;
			}
		}

		return randomCell;
	}
}