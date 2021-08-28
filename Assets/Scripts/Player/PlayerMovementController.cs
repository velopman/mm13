using UnityEngine;


public enum MovementState { DEFAULT, HANGING, JUMPING_UP }

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerCollisionController))]
[RequireComponent(typeof(PlayerInputController))]
public class PlayerMovementController : MonoBehaviour
{
    private PlayerClingController cling;
    private PlayerCollisionController collision;
    private PlayerInputController input;
    private Rigidbody2D rigidBody;

    public float dashForce = 20f;
    public float fallMultiplier = 4f;
    public float jumpForce = 15f;
    public float speed = 7.5f;

    public MovementState state;
    private Vector2 dashVelocity;
    private Vector2 jumpWallVelocity;


    // Lifecycle methods

    void Start()
    {
        this.cling = this.GetComponent<PlayerClingController>();
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
        switch (this.state)
        {
            case MovementState.DEFAULT:
                this.handleCling();
                if (this.state != MovementState.DEFAULT) return;

                this.dash();
                this.jump();
                this.jumpWall();
                this.move();
                this.handleGravity();

                break;

            case MovementState.HANGING:
                this.jumpUp();

                break;

            case MovementState.JUMPING_UP:
                this.handleGravity();
                this.move();

                if (this.collision.onGround)
                {
                    this.state = MovementState.DEFAULT;
                }

                break;
        }
    }


    // Private methods

    private void dash()
    {
        if (!this.input.dashTriggered || this.dashVelocity.magnitude > 0f) return;

        this.input.ResetDash();

        this.dashVelocity = new Vector2(this.jumpForce, 0f) * this.input.facing;
    }

    private void handleCling()
    {
        if(!this.cling.canCling) return;
        if(this.input.direction != this.cling.direction) return;
        if(this.rigidBody.velocity.y > 10f) return;

        this.state = MovementState.HANGING;
        this.rigidBody.velocity = Vector2.zero;
        this.rigidBody.gravityScale = 0f;
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

    private void jumpUp()
    {
        if(!this.input.jumpTriggered) return;

        this.rigidBody.velocity = Vector2.up * this.jumpForce;
        this.state = MovementState.JUMPING_UP;
    }

    private void jumpWall()
    {
        if (!this.input.jumpTriggered || this.collision.wasOnGround || !this.collision.onWall) return;

        this.input.ResetJump();

        this.jumpWallVelocity = Quaternion.Euler(0f, 0f, this.collision.wallDirection * 60f) * new Vector2(0f, this.jumpForce);
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
