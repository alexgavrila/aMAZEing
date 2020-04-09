using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public float pickUpRange = 0.5f;
    public string pickUpName;
    
    private Player player;
    
    // Update is called once per frame
    private void Update()
    {
        CheckIfPicked();
    }

    private void CheckIfPicked()
    {
        player = GameManager.instance.PlayerInstance;
        
        if (Vector3.Distance(player.transform.position, transform.position) <= 0.5f)
        {
            player.OnPickUp(this);
            
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickUpRange);    
    }
}
