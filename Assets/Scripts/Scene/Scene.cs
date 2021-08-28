using UnityEngine;

public class Scene : MonoBehaviour
{
    public SceneName identifer;
    public SceneName[] adjacent;

    private SceneSpawnLocation[] spawnLocations;

    void Start()
    {
        this.spawnLocations = this.GetComponentsInChildren<SceneSpawnLocation>();
    }

    public Vector2 respawn_location(Vector2 entry)
    {
        Vector2 closest = Vector2.positiveInfinity;

        foreach(var location in this.spawnLocations)
        {
            if (Vector2.Distance(location.transform.position, entry) < Vector2.Distance(closest, entry))
            {
                closest = location.transform.position;
            }
        }

        return closest;
    }
}
