using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
	#region Singleton
		public static GameManager instance;
	#endregion
	
	// The nav mesh object to bake at runtime
	public NavMeshSurface navMesh;
	
	public Maze mazePrefab;
	private Maze mazeInstance;

	public Player playerPrefab;

	public Player PlayerInstance
	{
		private set;
		get;
	}

	public EnemyManager enemyManagerPrefab;
	private EnemyManager enemyManager;

	// Keep a reference to the menu prefab
	public GameMenuUI menuObject;
	private GameMenuUI menuObjectInstance;

	private void Awake()
	{
		instance = this;
	}
	
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

	// Generate a new maze and compute the nav mesh agents' paths
	private void GenerateWorld()
	{
		mazeInstance = Instantiate(mazePrefab) as Maze;
		mazeInstance.Generate();
		
		navMesh.BuildNavMesh();
	}

	private void BeginGame()
	{
		GenerateWorld();
		
		PlayerInstance = Instantiate(playerPrefab) as Player;
		PlayerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));

		menuObjectInstance = Instantiate(menuObject)
			.SetPlayer(PlayerInstance);

		// Send the player reference to the enemy manager
		enemyManager = Instantiate(enemyManagerPrefab)
			.Init(PlayerInstance);

		//StartCoroutine(mazeInstance.Generate());
	}
	
	// Regenerate the world and set the player's new location
	private void RestartGame()
	{
		//StopAllCoroutines();
		Destroy(mazeInstance.gameObject);
		
		/*if (playerInstance != null)
		{
			Destroy(playerInstance.gameObject);
		}

		if (enemyManager != null)
		{
			Destroy(enemyManager.gameObject);
		}
		*/
		
		GenerateWorld();
		PlayerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
	}
}