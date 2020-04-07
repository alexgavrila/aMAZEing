using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Player playerInstance;
    private MazeRoom playerLastRoom;

    public EnemyController enemyPrefab;
    private List<EnemyController> enemies = new List<EnemyController>();

    public EnemyManager Init(Player player)
    {
        playerInstance = player;
        
        enemies.Add(Instantiate(enemyPrefab).SetPlayerReference(playerInstance));

        return this;
    }

    // Update is called once per frame
    void Update()
    {
        MazeCell playerCell = playerInstance.GetCellBelow();

        if (playerCell != null)
        {
            var room = playerCell.room;

            if (playerLastRoom != room)
            {
                playerLastRoom = room;

                foreach (var cell in room.GetRoomCells())
                {
                    cell.GetComponentInChildren<Renderer>().material.color = Color.red;
                }
            } 
        }
    }
}
