using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSceneController : MonoBehaviour
{
    public delegate void OnSceneChanged(Scene scene);
    public OnSceneChanged onSceneChanged;

    public LayerMask layerScene;
    public float collisionRadius = .25f;

    private CameraConfiner confiner;
    private Scene currentScene;
    private Vector2 entryLocation;

    private PlayerSaveDataController data;
    private bool loading;


    // Lifecycle methods

    void Awake()
    {
        var health = this.GetComponent<PlayerHealthController>();
        health.onDeath += this.onDeath;

        var camera = FindObjectOfType<Camera>();
        this.confiner = camera.GetComponent<CameraConfiner>();
        this.confiner.target = this.gameObject;

        this.data = this.GetComponent<PlayerSaveDataController>();
        this.data.onLoad += this.onLoad;

        SceneManager.sceneLoaded += this.onSceneLoaded;
    }

    void FixedUpdate()
    {
        if (this.loading) return;

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

        this.data.VisitScene(this.currentScene.identifer);

        if (this.onSceneChanged != null)
        {
            this.onSceneChanged(this.currentScene);
        }
    }

    // Private methods

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

    void unloadCurrent()
    {
        if (this.currentScene == null) return;

        SceneManager.UnloadSceneAsync(this.currentScene.identifer.ToString().ToLower());
    }


    // Callback methods

    private void onDeath(HealthController self, bool lethal)
    {
        if (lethal)
        {
            this.data.Load(true);
        }
        else
        {
            this.transform.position = this.currentScene.RespawnLocation(this.entryLocation);
        }
    }

    private void onLoad(PlayerSaveDataController.PlayerData data)
    {
        if (this.currentScene)
        {
            this.unloadCurrent();
            this.unloadAdjacent();
            this.currentScene = null;
        }

        SceneManager.LoadScene(data.currentScene.ToString().ToLower(), LoadSceneMode.Additive);
        this.loading = true;
    }

    private void onSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        var gameScene = scene.GetRootGameObjects()[0].GetComponent<Scene>();
        if (!gameScene) return;

        var pickedUp = this.data.GetPickedUpForScene(gameScene.identifer);
        gameScene.PickedUp(pickedUp);

        if (!this.loading) return;

        var location = gameScene.savePointLocation();
        this.transform.position = location;

        var collider = gameScene.GetComponent<BoxCollider2D>();
        this.confiner.SetCameraBounds(collider.bounds);
        this.confiner.SetCameraPosition(gameScene.transform.position);

        this.loading = false;
    }
}
