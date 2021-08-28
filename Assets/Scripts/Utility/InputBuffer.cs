using UnityEngine;
using UnityEngine.InputSystem;

public class InputBuffer
{
    private InputAction action;

    private float buffer = 0f;
    private float current = 0f;


    // Lifecycle methods

    public InputBuffer(InputAction _action, float _buffer)
    {
        this.action = _action;
        this.buffer = _buffer;
    }


    // Accessor methods

    public bool triggered
    {
        get
        {
            return this.current > 0f;
        }
    }


    // Public methods

    public void Reset()
    {
        this.current = 0f;
    }

    public void Update()
    {
        this.current = Mathf.Max(0f, this.current - Time.deltaTime);

        if (this.action.triggered)
        {
            this.current = this.buffer;
        }
    }
}
