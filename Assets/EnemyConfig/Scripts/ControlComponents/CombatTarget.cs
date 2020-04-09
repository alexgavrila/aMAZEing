using UnityEngine;
using UnityEngine.Events;

// Common Target component for the player and the enemies
public class CombatTarget : MonoBehaviour
{
    public float maxHealth = 100f;
    public float damage = 25f;

    public float currHealth;

    // Instead of implementing general logic, attach an event for death logic
    public UnityEvent onDeath;

    private bool isDead = false;
    
    public CombatTarget TakeDamage(float damageTaken)
    {
        currHealth -= damageTaken;

        if (currHealth <= 0)
        {
            // invoke it only once
            if (!isDead)
            {
                isDead = true;
                onDeath.Invoke();
            }
        }

        return this;
    }

    private void Start()
    {
        currHealth = maxHealth;
    }
}
