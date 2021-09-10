using UnityEngine;

public class SavePoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        var data = collider.GetComponent<PlayerSaveDataController>();
        if (data == null) return;

        data.Save();
    }
}
