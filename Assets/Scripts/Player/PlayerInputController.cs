using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction actionMove;

    private InputBuffer dashBuffer;
    private InputBuffer fallBuffer;
    private InputBuffer jumpBuffer;
    private InputBuffer meleeBuffer;
    private float moveDirection;
    private float facingDirection = 1f;

    // Lifecycle methods

    void Start()
    {
        this.playerInput = this.GetComponent<PlayerInput>();
        this.actionMove = this.playerInput.actions["move"];

        this.dashBuffer = new InputBuffer(this.playerInput.actions["dash"], .1f);
        this.fallBuffer = new InputBuffer(this.playerInput.actions["fall"], .1f);
        this.jumpBuffer = new InputBuffer(this.playerInput.actions["jump"], .1f);
        this.meleeBuffer = new InputBuffer(this.playerInput.actions["melee"], .1f);
    }

    void Update()
    {
        this.dashBuffer.Update();
        this.fallBuffer.Update();
        this.jumpBuffer.Update();
        this.meleeBuffer.Update();

        this.moveDirection = this.actionMove.ReadValue<float>();

        if (this.moveDirection != 0f)
        {
            this.facingDirection = this.moveDirection;
        }
    }


    // Accessor methods

    public bool dashTriggered
    {
        get
        {
            return this.dashBuffer.triggered;
        }
    }

    public bool fallTriggered
    {
        get
        {
            return this.fallBuffer.triggered;
        }
    }

    public bool jumpTriggered
    {
        get
        {
            return this.jumpBuffer.triggered;
        }
    }

    public bool meleeTriggered
    {
        get
        {
            return this.meleeBuffer.triggered;
        }
    }

    public float direction
    {
        get
        {
            return this.moveDirection;
        }
    }

    public float facing
    {
        get
        {
            return this.facingDirection;
        }
    }

    // Public methods

    public void ResetDash()
    {
        this.dashBuffer.Reset();
    }

    public void ResetFall()
    {
        this.fallBuffer.Reset();
    }

    public void ResetJump()
    {
        this.jumpBuffer.Reset();
    }

    public void ResetMelee()
    {
        this.meleeBuffer.Reset();
    }
}
