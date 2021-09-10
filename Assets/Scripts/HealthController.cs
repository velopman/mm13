using UnityEngine;

class HealthController : MonoBehaviour
{
    public delegate void OnDamaged(HealthController self, int health);
    public OnDamaged onDamaged;

    public delegate void OnDeath(HealthController self, bool lethal);
    public OnDeath onDeath;

    public int maxHealth = 1;
    protected int health = 0;


    // Lifecycle methods

    void Start()
    {
        this.health = this.maxHealth;
    }


    // Public methods

    public void damage(int amount, bool lethal = false)
    {
        this.health = Mathf.Max(0, this.health - amount);

        if (this.onDamaged != null)
        {
            this.onDamaged(this, this.health);
        }

        if (this.health == 0 && this.onDeath != null)
        {
            this.onDeath(this, lethal);
        }
    }
}
