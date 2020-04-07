using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public float rangeRadius = 10f;
    public float rotationSpd = 5f;
    
    // All enemies have the same target, the player
    private Player player;
    private NavMeshAgent agent;
    
    #region PublicApi
    
    public EnemyController SetPlayerReference(Player playerTarget)
    {
        player = playerTarget;

        return this;
    }
    
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateToPlayer().FollowPlayer();
    }

    private EnemyController FollowPlayer()
    {
        // Check if distance to player is within detection range
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= rangeRadius)
        {
            agent.SetDestination(player.transform.position);
        }

        return this;
    }

    private EnemyController RotateToPlayer()
    {
        var dirToPlayer = (player.transform.position - transform.position).normalized;
        dirToPlayer.y = 0;

        Quaternion rotateToPlayer = Quaternion.LookRotation(dirToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateToPlayer, rotationSpd * Time.deltaTime);
        
        return this;
    }
    
    // Visual feedback in the scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeRadius);
    }
}
