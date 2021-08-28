using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction actionMove;

    private InputBuffer dashBuffer;
    private InputBuffer jumpBuffer;
    private float moveDirection;
    private float facingDirection;

    // Lifecycle methods

    void Start()
    {
        this.playerInput = this.GetComponent<PlayerInput>();
        this.actionMove = this.playerInput.actions["move"];

        this.dashBuffer = new InputBuffer(this.playerInput.actions["dash"], .1f);
        this.jumpBuffer = new InputBuffer(this.playerInput.actions["jump"], .1f);
    }

    void Update()
    {
        this.dashBuffer.Update();
        this.jumpBuffer.Update();

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

    public bool jumpTriggered
    {
        get
        {
            return this.jumpBuffer.triggered;
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

    public void ResetJump()
    {
        this.jumpBuffer.Reset();
    }
}
