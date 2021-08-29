using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerInputController input;
    private new SpriteRenderer renderer;

    // Lifecycle methods

    void Start()
    {
        this.input = this.GetComponent<PlayerInputController>();
        this.renderer = this.GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        this.renderer.flipX = this.input.facing < 0f;
    }
}
