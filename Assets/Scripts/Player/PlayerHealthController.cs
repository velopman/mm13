using UnityEngine;


class PlayerHealthController : HealthController
{
    // Lifecycle Methods

    void Start()
    {
        this.health = this.maxHealth;

        this.onDeath += this.death;
    }


    // Callback methods

    void death(HealthController self, bool lethal)
    {
        this.health = this.maxHealth;

        if (this.onHealthChanged != null)
        {
            this.onHealthChanged(this.health);
        }
    }
}
