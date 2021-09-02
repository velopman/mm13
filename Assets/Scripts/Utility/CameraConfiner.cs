using UnityEngine;
using Cinemachine;

public class CameraConfiner : MonoBehaviour
{
    public GameObject target;
    public new BoxCollider2D collider;
    public Vector2 halfSize = new Vector2(11f, 5f);

    private Vector3 closestPoint;


    // Lifecycle methods

    void FixedUpdate()
    {
        if (this.collider == null) return;

        var min = (Vector2)this.collider.bounds.min + this.halfSize;
        var max = (Vector2)this.collider.bounds.max - this.halfSize;

        var x = Mathf.Clamp(this.target.transform.position.x, min.x, max.x);
        var y = Mathf.Clamp(this.target.transform.position.y, min.y, max.y);

        this.closestPoint = new Vector3(x, y, -100f);

        if (this.transform.position == this.closestPoint) return;

        var distance = Vector3.Distance(this.transform.position, this.closestPoint);
        var damping = Mathf.Clamp(distance * distance * 0.05f, 0.01f, 1.0f);

        this.transform.position = Vector3.MoveTowards(this.transform.position, this.closestPoint, Time.fixedDeltaTime * 75f * damping);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)this.closestPoint, .25f);
        Gizmos.DrawWireSphere((Vector2)this.transform.position, .25f);
    }


    // Private methods

    private bool isInside(Vector3 offset)
    {
        return this.collider.bounds.Contains(this.transform.position + offset);
    }
}
