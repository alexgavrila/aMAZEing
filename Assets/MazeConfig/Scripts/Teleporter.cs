using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public void SetCellPosition(MazeCell cell)
    {
        var pos = cell.transform.position;
        pos.y = transform.position.y;

        transform.position = pos;

    }

    void OnTriggerEnter(Collider o)
    {
        if(o.gameObject.tag == "Player")
        {
            GameManager.instance.RestartGame();
            o.GetComponent<Player>().ResetEnemiesKilled();
            Destroy(gameObject);
        }
    }
}
