using UnityEngine;
using UnityEngine.SceneManagement;

public class scene_controller : MonoBehaviour
{
    public LayerMask layerScene;
    public float collisionRadius = .25f;

    scene currentScene;

    // Update is called once per frame
    void Update()
    {
        var scene_collider = Physics2D.OverlapCircle((Vector2)this.transform.position, this.collisionRadius, this.layerScene);
        if (!scene_collider) return;

        var scene = scene_collider.GetComponent<scene>();

        var previousSceneIdentifier = SceneName.NONE;

        if (this.currentScene)
        {
            if (this.currentScene.identifer == scene.identifer) return;

            previousSceneIdentifier = this.currentScene.identifer;

            this.unload_adjacent(scene.identifer);
        }

        this.currentScene = scene;

        this.load_adjacent(previousSceneIdentifier);
    }

    void load_adjacent(SceneName excluding = SceneName.NONE)
    {
        foreach (var scene in this.currentScene.adjacent)
        {
            if (scene == excluding) continue;

            SceneManager.LoadSceneAsync(scene.ToString().ToLower(), LoadSceneMode.Additive);
        }
    }

    void unload_adjacent(SceneName excluding = SceneName.NONE)
    {
        foreach (var scene in this.currentScene.adjacent)
        {
            if (scene == excluding) continue;

            SceneManager.UnloadSceneAsync(scene.ToString().ToLower());
        }
    }
}
