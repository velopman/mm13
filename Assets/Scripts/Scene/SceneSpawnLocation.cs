using UnityEngine;

public class SceneSpawnLocation : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)this.transform.position, .25f);
    }
}
