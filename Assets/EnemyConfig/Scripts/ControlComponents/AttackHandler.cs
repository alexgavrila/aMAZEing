using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public enum AttackModes
    {
        Ranged
    }

    public float attackCooldownTime = 0.5f;
    
    [SerializeField] public AttackModes attackMode = AttackModes.Ranged;
    [SerializeField] public RifleController rifle;

    public PickUp coinPrefab;
    
    // The animator of the robot child object
    private Animator anim;
    private Player playerTarget;
    // Keep a reference to the attack coroutine so we can cancel it
    private Coroutine attackCoroutine;
    
    [NonSerialized]
    public bool canAttack = false;
    
    #region Constants
        private const string DeathAnimName = "Death";
        // Stop any attack anim currently playing
        private const string PassiveAnimName = "Passive";
        private const string AttackBoolName = "isAttacking";
    #endregion

    public AttackHandler SetTarget(Player player)
    {
        playerTarget = player;

        return this;
    }

    // If the enemy dies while attacking we should stop all attacking-related coroutines
    public void StopAttacking()
    {
        StopCoroutine(attackCoroutine);
        rifle.CancelShooting();
    }

    public void OnDeath()
    {
        playerTarget.OnEnemyKilled();
        Instantiate(coinPrefab);
        
        // Play the death animation and stop attacking
        anim.Play(PassiveAnimName);
        anim.Play(DeathAnimName);
        StopAttacking();

        StartCoroutine(AfterDeath());
    }

    private IEnumerator AfterDeath()
    {
        // Wait until the death animation finishes
        yield return new WaitUntil(() => IsAttackAnimOver(0));
    
        // deactivate collider
        GetComponentInChildren<BoxCollider>().enabled = false;
        
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        // Set a transparent shader so we can change the alpha of the object
        Shader transparentShader = Shader.Find("Legacy Shaders/Transparent/Bumped Diffuse");
        
        for (float alpha = 1; alpha >= 0; alpha -= 0.25f)
        {
            var render = GetComponentInChildren<SkinnedMeshRenderer>();
            render.material.shader = transparentShader;
            
            var color = render.material.color;
            color.a = alpha;
            
            render.material.color = color;
            
            
            yield return new WaitForSeconds(0.1f);
        }
        
        // After fade out complete, destroy the object
        Destroy(gameObject);
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            if (!canAttack)
            {
                yield return new WaitUntil(() => canAttack);
            }

            anim.SetBool(AttackBoolName, true);

            if (attackMode == AttackModes.Ranged)
            {
                rifle.Shoot(playerTarget);
            }

            yield return new WaitUntil(() => IsAttackAnimOver(1));
            
            anim.SetBool(AttackBoolName, false);

            yield return new WaitForSeconds(attackCooldownTime);
        }
    }

    private bool IsAttackAnimOver(int layerIndex)
    {
        return anim.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= 1;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        attackCoroutine = StartCoroutine(Attack());
    }
}
