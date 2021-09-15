using UnityEngine;

public class DummyDoor : MonoBehaviour
{
    public bool initialState;

    public Sprite activeSprite;
    public Sprite inactiveSprite;

    private BoxCollider2D collider;
    private SpriteRenderer renderer;


    // Lifecycle methods

    private void Start()
    {
        this.collider = this.GetComponent<BoxCollider2D>();
        this.renderer = this.GetComponent<SpriteRenderer>();

        this.SetState(this.initialState);
    }


    // Public methods

    public void SetState(bool locked)
    {
        this.collider.enabled = locked;
        this.renderer.sprite = locked ? inactiveSprite : activeSprite;
    }
}
