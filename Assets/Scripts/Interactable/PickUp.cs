using UnityEngine;

public class PickUp : MonoBehaviour
{
    public SceneName scene;
    public int id;

    // Lifecylce methods

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        var data = collider.GetComponent<PlayerSaveDataController>();
        if (data == null) return;

        data.PickUp(scene, id);

        Destroy(this.gameObject);
    }
}
