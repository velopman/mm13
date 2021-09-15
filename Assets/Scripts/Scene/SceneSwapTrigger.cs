using UnityEngine;

class SceneSwapTrigger : MonoBehaviour
{
    public SceneName unload;
    public SceneName load;


    // Lifecycle methods
    void OnTriggerEnter2D(Collider2D collider)
    {
        var sceneController = collider.GetComponent<PlayerSceneController>();
        if (sceneController == null) return;

        sceneController.UnloadScene(this.unload);
        sceneController.LoadScene(this.load);

        this.GetComponent<BoxCollider2D>().enabled = false;

        var door = this.GetComponentInParent<DummyDoor>();
        if (door == null) return;

        door.SetState(true);
    }
}
