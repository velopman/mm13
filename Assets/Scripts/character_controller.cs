using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(collision))]
public class character_controller : MonoBehaviour
{
    public float fallMultiplier = 5f;
    public float lowJumpMultiplier = 4f;

    public float speedGround = 10f;
    public float forceJump = 50f;

    private collision collision;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    private PlayerInput playerInput;
    private InputAction actionJump;
    private InputAction actionMove;

    private float __direction;

    void Start()
    {
        this.collision = this.GetComponent<collision>();
        this.rigidBody = this.GetComponent<Rigidbody2D>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        if (this.playerInput == null)
        {
            this.playerInput = this.GetComponent<PlayerInput>();
            this.actionJump = this.playerInput.actions["jump"];
            this.actionMove = this.playerInput.actions["move"];
        }

        this.walk();

        if (this.collision.onGround && this.actionJump.triggered)
        {
            this.jump();
        }

        var direction = this.actionMove.ReadValue<float>();

        this.__handleGravity();

        if (this.rigidBody.velocity.y < 0f && this.collision.onWall)
        {

            if ((this.collision.onWallLeft && direction < 0f) ||
                (this.collision.onWallRight && direction > 0f))
            {
                this.rigidBody.velocity = new Vector2(this.rigidBody.velocity.x, 0f);
            }
        }

        if (direction < 0f)
        {
            this.spriteRenderer.flipX = true;
        }
        else if (direction > 0f)
        {
            this.spriteRenderer.flipX = false;
        }

    }

    private void jump()
    {
        this.rigidBody.velocity = new Vector2(this.rigidBody.velocity.x, forceJump);
    }

    private void walk()
    {
        this.rigidBody.velocity = new Vector2(this.actionMove.ReadValue<float>()  * this.speedGround, this.rigidBody.velocity.y);
    }

    void __handleGravity()
    {
        Debug.Log(this.actionJump.ReadValue<float>());

        if(this.rigidBody.velocity.y < 0 || this.actionJump.ReadValue<float>() == 0f)
        {
            this.rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if(this.rigidBody.velocity.y > 0)
        {
            this.rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
