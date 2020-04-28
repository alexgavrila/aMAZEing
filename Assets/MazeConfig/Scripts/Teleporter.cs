using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{

    public Animator anim;
    public Image black;

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

            o.GetComponent<Player>().ResetEnemiesKilled();

            Destroy(gameObject);

            GameManager.instance.RestartGame();

            GameManager.instance.currentFloor++;
        }
    }

}
