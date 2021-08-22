using UnityEngine;
using UnityEngine.InputSystem;


public enum CharacterState { IDLE, WALKING, DASHING, JUMPING, WALL_JUMPING }

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(collision))]
public class character_controller : MonoBehaviour
{
    public float fallMultiplier = 4f;
    public float lowJumpMultiplier = 4f;

    public float speed = 7.5f;
    public float jumpForce = 15f;
    public float jumpDoubleForce = 12.5f;
    public float dashForce = 20f;

    [Header("Testing toggles")]
    public bool onWallToJump = true;
    public bool canDashOnGround = true;
    public bool canDashInAir = true;

    private collision collision;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    private PlayerInput playerInput;
    private InputAction actionJump;
    private InputAction actionMove;
    private InputAction actionDash;

    private float jumpBuffer = 0f;
    private bool canDoubleJump = true;
    private Vector2 jumpWallVelocity;

    private float dashBuffer = 0f;
    private bool canDash = true;
    private Vector2 dashVelocity;
    private float dashCooldown = 0f;

    scene currentScene;


    // Lifecycle methods

    void Start()
    {
        this.collision = this.GetComponent<collision>();
        this.rigidBody = this.GetComponent<Rigidbody2D>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.playerInput = this.GetComponent<PlayerInput>();
        this.actionJump = this.playerInput.actions["jump"];
        this.actionMove = this.playerInput.actions["move"];
        this.actionDash = this.playerInput.actions["dash"];
    }

    void Update()
    {
        this.jumpBuffer = Mathf.Max(0f, this.jumpBuffer - Time.deltaTime);

        if (this.actionJump.triggered)
        {
            this.jumpBuffer = .1f;
        }

        this.jumpWallVelocity = Vector2.MoveTowards(this.jumpWallVelocity, Vector2.zero, Time.deltaTime * 50f);

        this.dashBuffer = Mathf.Max(0f, this.dashBuffer - Time.deltaTime);

        if (this.actionDash.triggered)
        {
            this.dashBuffer = .1f;
        }

        this.canDash |= this.collision.onGround;

        this.dashVelocity = Vector2.MoveTowards(this.dashVelocity, Vector2.zero, Time.deltaTime * 50f);

        this.dashCooldown = Mathf.Max(0f, this.dashCooldown - Time.deltaTime);
    }

    void FixedUpdate()
    {
        this.dash();
        this.jump();
        this.jumpDouble();
        this.jumpWall();
        this.move();

        this.handleGravity();

        var direction = this.actionMove.ReadValue<float>();
        if (direction < 0f)
        {
            this.spriteRenderer.flipX = true;
        }
        else if (direction > 0f)
        {
            this.spriteRenderer.flipX = false;
        }
    }


    // Private methods

    private void dash()
    {
        var direction = this.actionMove.ReadValue<float>();

        if (direction == 0f) return;
        if (this.collision.onGround && (!this.canDashOnGround || this.dashCooldown > 0f)) return;
        if (!this.collision.onGround && (!this.canDashInAir || !this.canDash)) return;

        if (this.dashBuffer == 0f || this.dashVelocity.magnitude > 0f) return;

        this.dashBuffer = 0f;
        this.canDash = this.collision.onGround;

        if (this.collision.onGround)
        {
            this.dashCooldown = 1f;
        }

        this.dashVelocity = new Vector2(this.jumpForce, 0f) * direction;
    }

    private void handleGravity()
    {
        if(this.rigidBody.velocity.y < 0 || this.actionJump.ReadValue<float>() == 0f)
        {
            this.rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if(this.rigidBody.velocity.y > 0)
        {
            this.rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void jump()
    {
        if (!this.collision.onGround || this.jumpBuffer == 0f) return;

        this.canDoubleJump = true;
        this.jumpBuffer = 0f;

        this.dashVelocity = Vector2.zero;

        this.rigidBody.velocity = new Vector2(this.rigidBody.velocity.x, this.jumpForce);
    }

    private void jumpDouble()
    {
        if (this.collision.onGround || this.collision.onWall || !this.canDoubleJump || this.jumpBuffer == 0f) return;

        this.canDoubleJump = false;
        this.jumpBuffer = 0f;

        this.rigidBody.velocity = new Vector2(this.rigidBody.velocity.x, this.jumpDoubleForce);
    }

    private void jumpWall()
    {
        if (this.collision.onGround || !this.collision.onWall || this.jumpBuffer == 0f) return;

        if (this.onWallToJump)
        {
            var direction = this.actionMove.ReadValue<float>();
            var requiredDirection = this.collision.onWallLeft ? -1 : 1;

            if (direction != requiredDirection) return;
        }

        this.jumpBuffer = 0f;
        this.jumpWallVelocity = Quaternion.Euler(0f, 0f, this.collision.onWallRight ? 60f : -60f) * new Vector2(0f, this.jumpForce);
    }

    private void move()
    {
        var moveVelocity = new Vector2(this.actionMove.ReadValue<float>() * this.speed, this.rigidBody.velocity.y);

        if (this.jumpWallVelocity.magnitude > 0f)
        {
            moveVelocity.x *= .25f;
            moveVelocity.y = 0f;
        }

        this.rigidBody.velocity = this.jumpWallVelocity + this.dashVelocity + moveVelocity;
    }
}
