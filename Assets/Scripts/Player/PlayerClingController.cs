using UnityEngine;

public class PlayerClingController : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask layerGround;
    public float collisionRadius = .25f;
    public float collisionRadiusTop = .5f;

    public Vector2 offsetLeft, offsetTopLeft, offsetRight, offsetTopRight;


    [Header("State")]
    public bool canCling;
    public bool canClingLeft;
    public bool canClingRight;

    private Rigidbody2D rigidBody;


    // Lifecycle methods

    void Start()
    {
        this.rigidBody = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        this.canClingLeft = !this.checkCollision(this.offsetTopLeft, this.collisionRadiusTop) && this.checkCollision(this.offsetLeft, this.collisionRadius);
        this.canClingRight = !this.checkCollision(this.offsetTopRight, this.collisionRadiusTop) && this.checkCollision(this.offsetRight, this.collisionRadius);

        this.canCling = this.canClingLeft || this.canClingRight;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetLeft, this.collisionRadius);
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetTopLeft, this.collisionRadiusTop);
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetRight, this.collisionRadius);
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetTopRight, this.collisionRadiusTop);
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

    bool checkCollision(Vector2 offset, float radius)
    {
        return Physics2D.OverlapCircle((Vector2)this.rigidBody.position + offset, radius, this.layerGround);
    }
}
