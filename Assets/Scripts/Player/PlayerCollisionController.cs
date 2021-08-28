using UnityEngine;

public class PlayerCollisionController : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask layerGround;
    public float collisionRadius = .25f;
    public Vector2 offsetBottom, offsetLeft, offsetRight;


    [Header("State")]
    public bool onGround;
    public bool onWall;
    public bool onWallLeft;
    public bool onWallRight;


    // Lifecycle methods

    void Update()
    {
        this.onGround = Physics2D.OverlapCircle((Vector2)this.transform.position + this.offsetBottom, this.collisionRadius, this.layerGround);
        this.onWallLeft = Physics2D.OverlapCircle((Vector2)this.transform.position + this.offsetLeft, this.collisionRadius, this.layerGround);
        this.onWallRight = Physics2D.OverlapCircle((Vector2)this.transform.position + this.offsetRight, this.collisionRadius, this.layerGround);

        this.onWall = this.onWallLeft || this.onWallRight;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetBottom, this.collisionRadius);
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetLeft, this.collisionRadius);
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetRight, this.collisionRadius);
    }
}
