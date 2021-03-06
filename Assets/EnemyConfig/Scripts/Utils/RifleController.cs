﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CombatTarget))]
public class RifleController : MonoBehaviour
{
    public Transform shootOrigin;
    public float range = 5f;

    // Shooting particles
    public ParticleSystem flash;
    public GameObject hitFlare;

    // Time before the impact flare gets removed
    public float flareLifetime = 0.5f;

    // Wait time in frames before "bullet" animation after the fire animation started
    public int framesBeforeBullet = 11;

    private Coroutine shootingCoroutine;
    // Know if this rifle is currently shooting
    private bool isShooting = false;

    // the CombatTarget component on the parent Enemy container
    private CombatTarget combatTargetComponent;

    #region PositionConsants
    // Different positions and rotations for the different states of the character holding this rifle
    private readonly Vector3 firePosition = new Vector3(0.368f, -0.059f, -0.024f);
    private readonly Vector3 fireRotation = new Vector3(-46.456f, 93.266f, -196.42f);

    private readonly Vector3 walkPosition = new Vector3(0.394f, -0.0358f, 0.0005f);
    private readonly Vector3 walkRotation = new Vector3(-27.094f, 65.321f, -151.58f);

    private readonly Vector3 idlePosition = new Vector3(0.364f, -0.056f, -0.021f);
    private readonly Vector3 idleRotation = new Vector3(-14.422f, 96.594f, -166.348f);
    #endregion

    private AudioSource gunFire;

    #region PositionSetters
    public void SetInFirePosition()
    {
        SetLocalRotPos(firePosition, fireRotation);
    }

    public void SetInWalkingPosition()
    {
        if (isShooting)
        {
            return;
        }

        SetLocalRotPos(walkPosition, walkRotation);
    }

    public void SetInIdlePosition()
    {
        if (isShooting)
        {
            return;
        }

        SetLocalRotPos(idlePosition, idleRotation);
    }

    private void SetLocalRotPos(Vector3 pos, Vector3 rot)
    {
        this.transform.localPosition = pos;
        this.transform.localEulerAngles = rot;
    }
    #endregion

    // Ranges used when interpolating shooting vector
    // Difficulty determines how good the AI's aim is
    #region Difficulty Ranges
        private const float EasyStartRange = 0f;
        private const float EasyEndRange = 0.5f;

        private const float MediumStartRange = 0.3f;
        private const float MediumEndRange = 0.8f;

        private const float HardStartRange = 0.7f;
        private const float HardEndRange = 1f;
    #endregion

    public void Shoot(Player target)
    {
        isShooting = true;
        SetInFirePosition();

        shootingCoroutine = StartCoroutine(ShootingLogic(target));
    }

    public void CancelShooting()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
        }
    }

    // Get a random lerp value for the shooting aim depending on the game difficulty
    // the harder the difficulty the better the aim of the ai
    private float RandomShootingLerpVal()
    {
        float startRange, endRange;

        switch (GameMenuUI.Instance.gameDifficulty)
        {
            case GameMenuUI.GameDifficulty.Hard:
                startRange = HardStartRange;
                endRange = HardEndRange;
                break;
            case GameMenuUI.GameDifficulty.Medium:
                startRange = MediumStartRange;
                endRange = MediumEndRange;
                break;
            default:
                startRange = EasyStartRange;
                endRange = EasyEndRange;
                break;
        }

        return Random.Range(startRange, endRange);
    }

    private IEnumerator ShootingLogic(Player target)
    {
        yield return WaitForBullet();

        // Play bullet anims
        flash.Play();

        // play gunfire sound
        gunFire.Play();

        Vector3
            // the exact vector to the target, no miss
            vectorToTarget = target.transform.position - shootOrigin.transform.position,
            // the enemy's forward direction, hits the target only when the target is straight forward
            forwardDir = combatTargetComponent.transform.forward,
            shootDirection = Vector3.Lerp(forwardDir, vectorToTarget, RandomShootingLerpVal());

        // Check if the player was shot
        RaycastHit hit;
        if (Physics.Raycast(shootOrigin.position, shootDirection, out hit, range))
        {
            // Check if a combat target has been hit
            CombatTarget targetHit = hit.collider.gameObject.GetComponent<CombatTarget>();

            if (targetHit != null)
            {
                targetHit.TakeDamage(combatTargetComponent.damage);
            }

            // Play the flare particles on the hit target
            GameObject flareInstance = Instantiate(hitFlare, hit.point, Quaternion.LookRotation(hit.normal));
            flareInstance.GetComponent<ParticleSystem>().Play();
            Destroy(flareInstance, flareLifetime);
        }

        // Shots fired :)
        isShooting = false;
    }

    // Wait a number of frames before starting the fire animation
    // This way, the enemy doesn't shoot when the gun is down
    private IEnumerator WaitForBullet()
    {
        for (int i = 0; i <= framesBeforeBullet; i++)
        {
            yield return null;
        }
    }

    // When owner dies, he drops the weapon
    public void OnOwnerDeath()
    {
        transform.parent = null;
        var rigidBody = GetComponent<Rigidbody>();

        rigidBody.isKinematic = false;

        Destroy(gameObject, 1f);
    }

    // Start is called before the first frame update
    private void Start()
    {
        combatTargetComponent = GetComponentInParent<CombatTarget>();
        gunFire = GetComponent<AudioSource>();
    }
}
