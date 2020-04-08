using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AnimatorNavigation))]
[RequireComponent(typeof(EnemyVisibility))]
[RequireComponent(typeof(AttackHandler))]
public class EnemyController : MonoBehaviour
{
    // Interpolate between player current rotation and look rotation
    public float rotationSpd = 5f;
    
    // All enemies have the same target, the player
    private Player player;

    private bool isDead = false;
    
    #region ControlComponents
    // Check if the player is in attack range using this utility
    private EnemyVisibility visibilityCheck;
    // The component handling this object's movement
    private AnimatorNavigation navigation;
    // The component handling attacking
    private AttackHandler attack;
    #endregion

    #region PublicApi
        public EnemyController SetPlayerReference(Player playerTarget)
        {
            player = playerTarget;
            visibilityCheck.SetTargetReference(playerTarget);
            attack.SetTarget(playerTarget);

            return this;
        }

        // Stop following the player after death
        public void OnDeath()
        {
            isDead = true;
        }
    #endregion
    
    private void Awake()
    {
        visibilityCheck = GetComponent<EnemyVisibility>();
        navigation = GetComponent<AnimatorNavigation>();
        attack = GetComponent<AttackHandler>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDead)
        {
            return;
        }
        
        RotateToPlayer().FollowPlayer();
    }

    private EnemyController FollowPlayer()
    {
        // Detection range is larger then the view range
        // Also the player is being detected even if it is behind the enemy
        if (visibilityCheck.IsTargetDetected)
        {
            navigation.SetDestination(player.transform.position);

            // Check if the player is in attack range, i.e. we see it
            attack.canAttack = visibilityCheck.IsTargetVisible;
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

}
