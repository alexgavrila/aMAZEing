using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
	#region Singleton
		public static GameManager instance;
	#endregion

	// The nav mesh object to bake at runtime
	public NavMeshSurface navMesh;
	
	public Maze mazePrefab;

	public Maze mazeInstance
	{
		get;
		private set;
	}

	public Player playerPrefab;

	public Player PlayerInstance
	{
		private set;
		get;
	}

	public EnemyManager enemyManagerPrefab;
	public EnemyManager enemyManager;

	// Keep a reference to the menu prefab
	public GameMenuUI menuObject;
	private GameMenuUI menuObjectInstance;

	// Game over means the player has been killed
	// The death menu is shown
	public bool isGameOver = false;

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
			.Init(PlayerInstance)
			.ResetSpawnPoints();

		//StartCoroutine(mazeInstance.Generate());
	}
	
	// Regenerate the world and set the player's new location
	public void RestartGame(bool generateNewWorld = true)
	{
		//StopAllCoroutines();

		// Generate a new world and choose other spawn points
		if (generateNewWorld)
		{
			Destroy(mazeInstance.gameObject);
			GenerateWorld();
			enemyManager.ResetSpawnPoints();
		}

		/*if (playerInstance != null)
		{
			Destroy(playerInstance.gameObject);
		}

		if (enemyManager != null)
		{
			Destroy(enemyManager.gameObject);
		}
		*/
		
		PlayerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
	}
}