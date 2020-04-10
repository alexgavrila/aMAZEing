using UnityEngine;

public class PlayerRifle : MonoBehaviour
{
    public float range = 5f;
    public float fireRate = 5f;

    public float rifleDamage = 5f;

    private float timeBetweenShots;
    private float lastShotTime = 0f;
    
    // Shooting particles
    public ParticleSystem flash;
    public GameObject hitFlare;
    
    // Time before the impact flare gets removed
    public float flareLifetime = 0.5f;
    
    // the CombatTarget component on the parent Player container
    private CombatTarget combatTargetComponent;
    // the parent camera
    private Camera mainCamera;

    // Offset the shooting origin otherwise we will shoot ourselves
    private float zShootingOffset;
    
    public void Shoot()
    {
        if (Time.time - lastShotTime < timeBetweenShots)
        {
            return;
        }
        
        lastShotTime = Time.time;
        
        // Play bullet anims
        flash.Play();
        
        // Check the hit target
        Vector3 vectorToTarget = mainCamera.transform.forward;
        
        RaycastHit hit;
        var origin = mainCamera.transform.position;
        
        // Ignore player's layer
        int mask = LayerMask.GetMask("Player");
        mask = ~mask;
        
        if (Physics.Raycast(origin, vectorToTarget, out hit, range, mask))
        {
            // Check if a combat target has been hit
            CombatTarget targetHit = hit.collider.gameObject.GetComponentInParent<CombatTarget>();

            if (targetHit != null)
            {
                // Make sure we don't shoot ourselves
                if (targetHit != combatTargetComponent)
                {
                    targetHit.TakeDamage(combatTargetComponent.damage);
                }
            }

            // Play the flare particles on the hit target
            GameObject flareInstance = Instantiate(hitFlare, hit.point, Quaternion.LookRotation(hit.normal));
            flareInstance.GetComponent<ParticleSystem>().Play();
            Destroy(flareInstance, flareLifetime);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        combatTargetComponent = GetComponentInParent<CombatTarget>();
        mainCamera = GetComponentInParent<Camera>();
        
        timeBetweenShots = 1f / fireRate;
        
        // override combatTarget component's damage
        combatTargetComponent.damage = rifleDamage;
    }
}
