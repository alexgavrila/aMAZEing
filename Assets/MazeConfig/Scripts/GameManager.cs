using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
	// The nav mesh object to bake at runtime
	public NavMeshSurface navMesh;
	
	public Maze mazePrefab;
	private Maze mazeInstance;

	public Player playerPrefab;
	private Player playerInstance;

	public EnemyManager enemyManagerPrefab;
	private EnemyManager enemyManager;
	
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
		
		navMesh.BuildNavMesh();
		
		playerInstance = Instantiate(playerPrefab) as Player;
		playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
		
		// Send the player reference to the enemy manager
		enemyManager = Instantiate(enemyManagerPrefab)
			.Init(playerInstance);

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

		if (enemyManager != null)
		{
			Destroy(enemyManager.gameObject);
		}
		
		BeginGame();
	}
}