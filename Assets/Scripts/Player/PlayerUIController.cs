using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    TMPro.TextMeshPro help;
    TMPro.TextMeshPro stats;


    private PlayerInputController input;

    private bool hasMoved, hasJumped;

    private PlayerAbility ability;
    private float abilityTimer;

    private bool hasPickUp, hasEnemy, hasFountain;
    private float pickUpTimer, enemyTimer, fountainTimer;

    private int health, score;
    private float deathTimer;

    // Lifecycle methods

    void Start()
    {
        this.input = this.GetComponent<PlayerInputController>();

        var ability = this.GetComponent<PlayerAbilityController>();
        ability.onAbilityUnlocked += this.onAbilityUnlocked;

        var scene = this.GetComponent<PlayerSceneController>();
        scene.onSceneChanged += this.onSceneChanged;

        var health = this.GetComponent<PlayerHealthController>();
        this.health = health.maxHealth;
        health.onHealthChanged += this.onHealthChanged;
        health.onDeath += this.onDeath;

        var score = this.GetComponent<PlayerScoreController>();
        score.onScoreUpdate += this.onScoreUpdate;

        var camera = FindObjectOfType<Camera>();
        var text = camera.GetComponentsInChildren<TMPro.TextMeshPro>();

        this.help = text[0];
        this.help.text = "";

        this.stats = text[1];
        this.stats.text = "Health: " + this.health + "\nScore: " + this.score;
    }

    void FixedUpdate()
    {
        this.stats.text = "Health: " + this.health + "\nScore: " + this.score;


        this.hasMoved = this.hasMoved || this.input.direction != 0;
        this.hasJumped = this.hasJumped || this.input.jumpTriggered;

        if (this.deathTimer > 0f)
        {
            this.deathTimer = Mathf.Max(0f, this.deathTimer - Time.fixedDeltaTime);

            this.help.text = "You died and returned to last fountain";
            return;
        }


        if (!this.hasMoved)
        {
            this.help.text = "Press arrow keys to move";
            return;
        }

        if (!this.hasJumped)
        {
            this.help.text = "Press space to jump";
            return;
        }

        if (this.pickUpTimer > 0f)
        {
            this.pickUpTimer = Mathf.Max(0f, this.pickUpTimer - Time.fixedDeltaTime);

            this.help.text = "Collect diamonds to score points!";

            return;
        }

        if (this.enemyTimer > 0f)
        {
            this.enemyTimer = Mathf.Max(0f, this.enemyTimer - Time.fixedDeltaTime);

            this.help.text = "Press F to attack";

            return;
        }

        if (this.fountainTimer > 0f)
        {
            this.fountainTimer = Mathf.Max(0f, this.fountainTimer - Time.fixedDeltaTime);

            this.help.text = "Visit fountains to save progress";

            return;
        }


        if (this.abilityTimer > 0f)
        {
            this.abilityTimer = Mathf.Max(0f, this.abilityTimer - Time.fixedDeltaTime);

            switch(this.ability)
            {
                case PlayerAbility.DASH:
                    this.help.text = "Dash unlocked! - Press shift to dash";
                    break;
                case PlayerAbility.WALL_JUMP:
                    this.help.text = "Wall jump unlocked! - Press space near a wall";
                    break;
                case PlayerAbility.HANG:
                    this.help.text = "Ledge hang unlocked! - Hold direction near ledge";
                    break;
            }

            return;
        }

        this.help.text = "";
    }


    // Callback methods
    void onAbilityUnlocked(PlayerAbility ability)
    {
        this.ability = ability;
        this.abilityTimer = 3f;
    }

    void onDeath(HealthController self, bool lethal)
    {
        if (lethal)
        {
            this.deathTimer = 3f;
        }
    }

    void onHealthChanged(int health)
    {
        this.health = health;
    }

    void onSceneChanged(Scene scene)
    {
        if (!this.hasPickUp && scene.HasPickUp())
        {
            this.hasPickUp = true;
            this.pickUpTimer = 3f;
        }

        if (!this.hasEnemy && scene.HasEnemy())
        {
            this.hasEnemy = true;
            this.enemyTimer = 3f;
        }

        if (!this.hasFountain && scene.HasFountain())
        {
            this.hasFountain = true;
            this.fountainTimer = 3f;
        }
    }

    void onScoreUpdate(int score)
    {
        this.score = score;
    }
}
