using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSceneController : MonoBehaviour
{
    public LayerMask layerScene;
    public float collisionRadius = .25f;

    private CameraConfiner confiner;
    private Scene currentScene;
    private Vector2 entryLocation;


    // Lifecycle methods

    void Start()
    {
        var health = this.GetComponent<PlayerHealthController>();
        health.onDeath += this.onDeath;

        var camera = FindObjectOfType<Camera>();
        this.confiner = camera.GetComponent<CameraConfiner>();
        this.confiner.target = this.gameObject;
    }


    void Update()
    {
        var scene_collider = Physics2D.OverlapCircle((Vector2)this.transform.position, this.collisionRadius, this.layerScene);
        if (!scene_collider) return;

        var scene = scene_collider.GetComponent<Scene>();

        var previousSceneIdentifier = SceneName.NONE;

        if (this.currentScene)
        {
            if (this.currentScene.identifer == scene.identifer) return;

            previousSceneIdentifier = this.currentScene.identifer;

            this.unloadAdjacent(scene.identifer);
        }

        this.currentScene = scene;
        this.entryLocation = this.transform.position;

        this.confiner.SetCameraBounds(scene_collider.bounds);

        this.loadAdjacent(previousSceneIdentifier);
    }


    // Private Methods

    void loadAdjacent(SceneName excluding = SceneName.NONE)
    {
        foreach (var scene in this.currentScene.adjacent)
        {
            if (scene == excluding) continue;

            SceneManager.LoadSceneAsync(scene.ToString().ToLower(), LoadSceneMode.Additive);
        }
    }

    void unloadAdjacent(SceneName excluding = SceneName.NONE)
    {
        foreach (var scene in this.currentScene.adjacent)
        {
            if (scene == excluding) continue;

            SceneManager.UnloadSceneAsync(scene.ToString().ToLower());
        }
    }


    // Callback methods

    private void onDeath(HealthController self)
    {
        this.transform.position = this.currentScene.respawn_location(this.entryLocation);
    }
}
