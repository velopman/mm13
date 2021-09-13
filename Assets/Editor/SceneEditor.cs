using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Scene))]
public class SceneEditor : Editor
{
    private Scene scene;

    private void OnEnable()
    {
        this.scene = this.target as Scene;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update scene trigger"))
        {
            this.scene.UpdateTrigger();
        }

        if (GUILayout.Button("Update spawn locations"))
        {
            this.scene.UpdateSpawnLocations();
        }
    }
}
