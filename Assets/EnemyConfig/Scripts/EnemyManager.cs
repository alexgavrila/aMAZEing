using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    private Player playerInstance;

    public EnemyController enemyPrefab;
    private List<EnemyController> enemies = new List<EnemyController>();
    
    // Randomly chosen rooms 
    private List<MazeRoom> spawnRooms;
    // The number of enemies left to spawn inside each room
    private List<int> enemiesLeftToSpawn;
    
    public EnemyManager Init(Player player)
    {
        playerInstance = player;

        return this;
    }

    public EnemyManager ResetSpawnPoints()
    {
        List<MazeRoom> generatedRooms = GameManager.instance.mazeInstance.rooms;

        var roomsSpawningEnemies = Math.Min(LevelParams.roomsSpawningEnemies, generatedRooms.Count);

        spawnRooms = new List<MazeRoom>();
        
        for (int i = 0; i < roomsSpawningEnemies; i++)
        {
            int randIdx = 0, tries = 0;
            // We might not find another spawning room.
            // In this case stop trying and keep the rooms picked until now
            bool roomFound = true;
            
            do
            {
                randIdx = Random.Range(0, generatedRooms.Count - 1);
                tries++;

                if (tries >= generatedRooms.Count)
                {
                    roomFound = false;
                    break;
                }
            } while (
                AlreadyInSpawnRooms(generatedRooms, randIdx) || generatedRooms[randIdx].RoomSize() < LevelParams.minRoomSize
                );

            if (!roomFound)
            {
                break;
            }
            
            var spawnRoom = generatedRooms[randIdx];
            
            spawnRooms.Add(spawnRoom);

            // Color the floor 
            MarkSpawningRoom(spawnRoom);
        }
        
        // Make sure we have at least one enemy spawning per room
        var enemiesPerRoom = Math.Max(
            (int)Math.Ceiling((float)LevelParams.enemiesToKill / spawnRooms.Count),
            1
            );
        enemiesLeftToSpawn = spawnRooms.Select(room => enemiesPerRoom).ToList();

        return this;
    }

    public void DespawnEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null && enemy.gameObject != null)
            {
                Destroy(enemy.gameObject);
            }
        }

        enemies = new List<EnemyController>();
    }

    // Check if this room has already been chosen as a spawn point
    private bool AlreadyInSpawnRooms(List<MazeRoom> generatedRooms, int idx)
    {
        var room = generatedRooms[idx];

        foreach (var spawnRoom in spawnRooms)
        {
            if (spawnRoom == room)
            {
                return true;
            }
        }
        
        return false;
    }

    private void MarkSpawningRoom(MazeRoom room)
    {
        foreach (var cell in room.GetRoomCells())
        {
            cell.GetComponentInChildren<Renderer>().material.color = LevelParams.spawnRoomsFloorColor;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        SpawnEnemy();
    }

    // Check the room the player is in
    private void SpawnEnemy()
    {
        MazeCell playerCell = playerInstance.GetCellBelow();

        if (playerCell != null)
        {
            var room = playerCell.room;

            for (var i = 0; i < spawnRooms.Count; i++)
            {
                var spawnRoom = spawnRooms[i];
                
                // If the player is in a spawning room and there are enemies left to spawn, spawn enemy
                if (spawnRoom == room && enemiesLeftToSpawn[i] > 0)
                {
                    MazeCell emptyCellInRoom = null;
                    do
                    {
                        emptyCellInRoom = room.PickRandomEmptyCell();
                    } while (playerCell == emptyCellInRoom);

                    enemies.Add(
                        Instantiate(enemyPrefab)
                            .SetPlayerReference(playerInstance)
                            .SetCellPosition(emptyCellInRoom)
                        );
                    
                    enemiesLeftToSpawn[i]--;
                }
            }
        }
    }
}
