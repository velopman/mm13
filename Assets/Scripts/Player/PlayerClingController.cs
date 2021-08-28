using UnityEngine;

public class PlayerClingController : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask layerGround;
    public float collisionRadius = .25f;

    public Vector2 offsetLeft, offsetTopLeft, offsetRight, offsetTopRight;


    [Header("State")]
    public bool canCling;
    public bool canClingLeft;
    public bool canClingRight;


    // Lifecycle methods

    void Update()
    {
        this.canClingLeft = this.checkCollision(this.offsetLeft) && !this.checkCollision(this.offsetTopLeft);
        this.canClingRight = this.checkCollision(this.offsetRight) && !this.checkCollision(this.offsetTopRight);

        this.canCling = this.canClingLeft || this.canClingRight;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetLeft, this.collisionRadius);
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetTopLeft, this.collisionRadius);
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetRight, this.collisionRadius);
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetTopRight, this.collisionRadius);
    }


    // Accessor methods

    public float direction
    {
        get
        {
            return this.canClingLeft ? -1f : 1f;
        }
    }


    // Private methods

    bool checkCollision(Vector2 offset)
    {
        return Physics2D.OverlapCircle((Vector2)this.transform.position + offset, this.collisionRadius, this.layerGround);
    }
}
