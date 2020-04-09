using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : MazePassage
{

	public Transform hinge;

	private MazeDoor OtherSideOfDoor
	{
		get
		{
			return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
		}
	}

	public override void Initialize(MazeCell primary, MazeCell other, MazeDirection direction)
	{
		base.Initialize(primary, other, direction);
		
		// If there are two doors, we cannot play the door opening animation
		/*if (OtherSideOfDoor != null)
		{
			hinge.localScale = new Vector3(-1f, 1f, 1f);
			Vector3 p = hinge.localPosition;
			p.x = -p.x;
			hinge.localPosition = p;
		}*/
		
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (child != hinge)
			{
				child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
			}
			// destroy the door
			else if (OtherSideOfDoor != null)
			{
				Destroy(child.gameObject);
			}
		}
	}





}
