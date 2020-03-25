using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public Maze mazePrefab;

	private Maze mazeInstance;

	public Player playerPrefab;

	private Player playerInstance;

	private void Start()
	{
		BeginGame();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.CapsLock))
		{
			RestartGame();
		}
	}

	private void BeginGame()
	{
		
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.Generate();
		playerInstance = Instantiate(playerPrefab) as Player;
		playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
		
		//StartCoroutine(mazeInstance.Generate());
	}

	private void RestartGame()
	{
		//StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		if (playerInstance != null)
		{
			Destroy(playerInstance.gameObject);
		}
		BeginGame();
	}
}