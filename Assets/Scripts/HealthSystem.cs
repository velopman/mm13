public class HealthSystem{
    private int maxHealth;
    private int health;

    public HealthSystem(int maxHealth){
        this.maxHealth = maxHealth;
        this.health = maxHealth;
    }

    public int getHealth(){
        return health;
    }

    public void SetHealth(int valueChange){
        health += valueChange;
    }
}
