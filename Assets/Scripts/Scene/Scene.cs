using UnityEngine;

public class Scene : MonoBehaviour
{
    public SceneName identifer;
    public SceneName[] adjacent;

    private SceneSpawnLocation[] spawnLocations;
    private SavePoint savePoint;


    // Lifecycle methods

    void Awake()
    {
        this.spawnLocations = this.GetComponentsInChildren<SceneSpawnLocation>();
        this.savePoint = this.GetComponentInChildren<SavePoint>();
    }


    // Public methods

    public Vector2 RespawnLocation(Vector2 entry)
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

    public Vector2 savePointLocation()
    {
        if (this.savePoint)
        {
            return this.savePoint.transform.position;
        }

        return this.spawnLocations[0].transform.position;
    }
}
