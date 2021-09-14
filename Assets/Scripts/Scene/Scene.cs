using UnityEngine;
using UnityEngine.Tilemaps;


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

        var pickups = this.GetComponentsInChildren<PickUp>();

        for (int i = 0; i < pickups.Length; ++i)
        {
            pickups[i].scene = this.identifer;
            pickups[i].id = 1 << i;
        }
    }


    // Public methods


    public void PickedUp(int pickedUp)
    {
        var pickups = this.GetComponentsInChildren<PickUp>();

        for (int i = 0; i < pickups.Length; ++i)
        {
            var identifier = 1 << i;

            if ((pickedUp & identifier) != 0)
            {
                Destroy(pickups[i].gameObject);
            }
        }
    }

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


    // Editor methods

    public void UpdateTrigger()
    {
        var tilemap = this.GetComponentInChildren<Tilemap>();
        tilemap.CompressBounds();
        var bounds = tilemap.localBounds;

        var collider = this.GetComponent<BoxCollider2D>();
        collider.offset = bounds.center;
        collider.size = bounds.size;

        // for ( int y = (int) bounds.min.y; y < bounds.max.y; ++y )
        // {
        //     for ( int x = (int) bounds.min.x; x < bounds.max.x; ++x )
        //     {
        //         var tile = tilemap.GetTile(new Vector3Int(x, y, 0));
        //         tile.GetTileData()
        //     }
        // }
    }

    public void UpdateSpawnLocations()
    {
        foreach (var spawnLoaction in this.GetComponentsInChildren<SceneSpawnLocation>())
        {
            DestroyImmediate(spawnLoaction.gameObject);
        }


        var tilemap = this.GetComponentInChildren<Tilemap>();
        tilemap.CompressBounds();
        var bounds = tilemap.localBounds;

        // Create spawn locations on left
        for (var position = new Vector3Int((int) bounds.min.x, (int) bounds.min.y, 0); position.y < bounds.max.y; ++position.y)
        {
            if (tilemap.HasTile(position)) continue;

            var start = position;
            do
            {
                ++position.y;
            }
            while (position.y < bounds.max.y && !tilemap.HasTile(position));

            this.createSpawnLocation("left", start, position, Vector3.right);
        }

        // Create spawn locations on right
        for (var position = new Vector3Int((int) bounds.max.x - 1, (int) bounds.min.y, 0); position.y < bounds.max.y; ++position.y)
        {
            if (tilemap.HasTile(position)) continue;

            var start = position;
            do
            {
                ++position.y;
            }
            while (position.y < bounds.max.y && !tilemap.HasTile(position));

            this.createSpawnLocation("right", start, position, Vector3.zero);
        }

        // Create spawn locations on roof
        for (var position = new Vector3Int((int) bounds.min.x, (int) bounds.max.y - 1, 0); position.x < bounds.max.x; ++position.x)
        {
            if (tilemap.HasTile(position)) continue;

            var start = position;
            do
            {
                ++position.x;
            }
            while (position.x < bounds.max.x && !tilemap.HasTile(position));

            this.createSpawnLocation("roof", start, position, Vector3.zero);
        }

        // Create spawn locations on floor
        for (var position = new Vector3Int((int) bounds.min.x, (int) bounds.min.y, 0); position.x < bounds.max.x; ++position.x)
        {
            if (tilemap.HasTile(position)) continue;

            var start = position;
            do
            {
                ++position.x;
            }
            while (position.x < bounds.max.x && !tilemap.HasTile(position));

            this.createSpawnLocation("floor", start, position, Vector3.up);
        }
    }

    private void createSpawnLocation(string name, Vector3 start, Vector3 end, Vector3 offset)
    {
        var delta = end - start;

        var go = new GameObject("spawn_location (" + name + ")");
        go.transform.parent = this.transform;
        go.transform.localPosition = start + delta * .5f + offset;

        go.AddComponent<SceneSpawnLocation>();
    }
}
