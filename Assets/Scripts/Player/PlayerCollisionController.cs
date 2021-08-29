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


    private float onGroundTime = 0f;


    // Lifecycle methods

    void FixedUpdate()
    {
        this.onGround = this.checkCollision(this.offsetBottom);
        if (this.onGround)
        {
            this.onGroundTime = .15f;
        }
        else
        {
            this.onGroundTime = Mathf.Max(0f, this.onGroundTime - Time.deltaTime);
        }

        this.onWallLeft = this.checkCollision(this.offsetLeft);
        this.onWallRight = this.checkCollision(this.offsetRight);

        this.onWall = this.onWallLeft || this.onWallRight;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetBottom, this.collisionRadius);
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetLeft, this.collisionRadius);
        Gizmos.DrawWireSphere((Vector2)this.transform.position + this.offsetRight, this.collisionRadius);
    }


    // Accessor methods

    public bool wasOnGround
    {
        get
        {
            return this.onGroundTime > 0f;
        }
    }

    public float wallDirection
    {
        get
        {
            return this.onWallLeft ? -1f : 1f;
        }
    }


    // Private methods

    bool checkCollision(Vector2 offset)
    {
        return Physics2D.OverlapCircle((Vector2)this.transform.position + offset, this.collisionRadius, this.layerGround);
    }
}
