using UnityEngine;


class PlayerHealthController : HealthController
{
    // Lifecycle Methods

    void Start()
    {
        this.health = this.maxHealth;

        this.onDamaged += this.debugDamaged;
        this.onDeath += this.debugDeath;
        this.onDeath += this.death;
    }


    // Callback methods
    void death(HealthController self)
    {
        this.health = this.maxHealth;
    }


    // Test methods

    void debugDamaged(HealthController self, int health)
    {
        Debug.Log("Damaged, health remaining: " + health);
    }

    void debugDeath(HealthController self)
    {
        Debug.Log("Deaded");
    }
}
