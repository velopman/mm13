using UnityEngine;

public class CameraConfiner : MonoBehaviour
{
    public GameObject target;
    public Vector2 screenSize = new Vector2(22f, 12f);
    public Vector2 cameraDeadZone = new Vector2(16f, 6f);

    private bool boundsSet;
    private Bounds cameraBounds;
    private Bounds targetBounds;

    private Vector2 closestPointCamera;
    private Vector2 closestPointTarget;


    // Lifecycle methods

    void Start()
    {
        this.targetBounds = new Bounds(
            Vector3.zero,
            this.cameraDeadZone
        );
    }

    void FixedUpdate()
    {
        if (!this.boundsSet) return;

        var cameraPosition = (Vector2) this.transform.position;
        var targetPosition = (Vector2) this.target.transform.position;


        this.targetBounds.center = this.transform.position;
        this.closestPointTarget = this.targetBounds.ClosestPoint(targetPosition);

        var cameraTargetPosition = cameraPosition;
        cameraTargetPosition += targetPosition - this.closestPointTarget;

        this.closestPointCamera = this.cameraBounds.ClosestPoint(cameraTargetPosition);

        var distance = Vector3.Distance(cameraPosition, this.closestPointCamera) + .5f;
        var damping = Mathf.Clamp(distance * distance * .05f, 0.01f, 1.0f);

        var desiredPosition = (Vector3) this.closestPointCamera;
        desiredPosition.z = this.transform.position.z;

        this.transform.position = Vector3.MoveTowards(
            this.transform.position,
            desiredPosition,
            Time.fixedDeltaTime * 75f * damping
        );
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)this.closestPointTarget, .25f);
        Gizmos.DrawWireSphere((Vector2)this.closestPointCamera, .25f);
        Gizmos.DrawWireSphere((Vector2)this.transform.position, .25f);
        Gizmos.DrawWireCube((Vector2)this.cameraBounds.center, this.cameraBounds.size);
        Gizmos.DrawWireCube((Vector2)this.targetBounds.center, this.targetBounds.size);
    }

    // Public methods

    public void SetCameraBounds(Bounds bounds)
    {
        this.boundsSet = true;
        this.cameraBounds = new Bounds(
            bounds.center,
            (Vector2)bounds.size - this.screenSize
        );
    }

    public void SetCameraPosition(Vector3 position)
    {
        position.z = this.transform.position.z;
        this.transform.position = position;
    }
}
