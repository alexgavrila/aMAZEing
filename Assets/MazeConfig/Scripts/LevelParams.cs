using UnityEngine;

// A place where all level parameters are kept
public class LevelParams : ScriptableObject
{
    // How many enemies the player has to kill this level
    public static int enemiesToKill = 3;
    // How many rooms will spawn enemies this level
    // (will be clamped to the number of rooms the maze has)
    public static int roomsSpawningEnemies = 3;
    
    // make all the spawning rooms' floor this color so we can identify them
    public static Color spawnRoomsFloorColor = Color.white;

    // The minimum size (in cells) of a spawning room
    public static int minRoomSize = 15;
}

