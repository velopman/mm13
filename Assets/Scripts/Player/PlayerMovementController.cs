using UnityEngine;


public enum CharacterState { IDLE, WALKING, DASHING, JUMPING, WALL_JUMPING }

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerCollisionController))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerMovementController : MonoBehaviour
{
    private PlayerCollisionController collision;
    private PlayerInputController input;
    private Rigidbody2D rigidBody;

    public float dashForce = 20f;
    public float fallMultiplier = 4f;
    public float jumpForce = 15f;
    public float speed = 7.5f;

    private Vector2 dashVelocity;
    private Vector2 jumpWallVelocity;


    // Lifecycle methods

    void Start()
    {
        this.collision = this.GetComponent<PlayerCollisionController>();
        this.input = this.GetComponent<PlayerInputController>();
        this.rigidBody = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        this.jumpWallVelocity = Vector2.MoveTowards(this.jumpWallVelocity, Vector2.zero, Time.deltaTime * 50f);
        this.dashVelocity = Vector2.MoveTowards(this.dashVelocity, Vector2.zero, Time.deltaTime * 20f);
    }

    void FixedUpdate()
    {
        this.dash();
        this.jump();
        this.jumpWall();
        this.move();
        this.handleGravity();
    }


    // Private methods

    private void dash()
    {
        if (!this.input.dashTriggered || this.dashVelocity.magnitude > 0f) return;

        this.input.ResetDash();

        this.dashVelocity = new Vector2(this.jumpForce, 0f) * this.input.facing;
    }

    private void handleGravity()
    {
        this.rigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    }

    private void jump()
    {
        if (!this.input.jumpTriggered || !this.collision.wasOnGround) return;

        this.input.ResetJump();

        this.rigidBody.velocity = new Vector2(this.rigidBody.velocity.x, this.jumpForce);
    }

    private void jumpWall()
    {
        if (!this.input.jumpTriggered || this.collision.wasOnGround || !this.collision.onWall) return;

        this.input.ResetJump();

        this.jumpWallVelocity = Quaternion.Euler(0f, 0f, this.collision.onWallRight ? 60f : -60f) * new Vector2(0f, this.jumpForce);
    }

    private void move()
    {
        var moveVelocity = new Vector2(this.input.direction * this.speed, this.rigidBody.velocity.y);

        if (this.jumpWallVelocity.magnitude > 0f)
        {
            moveVelocity.x *= .25f;
            moveVelocity.y = 0f;
        }

        this.rigidBody.velocity = this.jumpWallVelocity + this.dashVelocity + moveVelocity;
    }
}
