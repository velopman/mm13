using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

class PlayerSaveDataController : MonoBehaviour
{
    [Serializable]
    public struct SceneData
    {
        public SceneName name;
        public int pickedUp;
    }

    [Serializable]
    public class PlayerData
    {
        public List<SceneData> visitedScenes;
        public SceneName currentScene;
        public PlayerAbility unlockedAbilities;
        public int score;

        public PlayerData()
        {
            this.visitedScenes = new List<SceneData>();
        }
    }

    public delegate void OnLoad(PlayerData data);
    public OnLoad onLoad;

    public SceneName defaultScene;
    public string saveName = "save_file";
    public bool loadFromFile;

    private PlayerData data;


    // Lifecycle methods
    void Awake()
    {
        this.Load();
    }

    // Public methods

    public void VisitScene(SceneName scene)
    {
        if (!this.HasVisitedScene(scene))
        {
            this.data.visitedScenes.Add(new SceneData { name = scene });
        }

        this.data.currentScene = scene;
    }

    public void SetUnlockedAbilities(PlayerAbility unlockedAbilities)
    {
        this.data.unlockedAbilities = unlockedAbilities;
    }

    public void SetScore(int score)
    {
        this.data.score = score;
    }

    public void PickUp(SceneName scene, int pickUp)
    {
        for (int i = 0; i < this.data.visitedScenes.Count; ++i)
        {
            if (this.data.visitedScenes[i].name != scene) continue;

            var visitedScene = this.data.visitedScenes[i];
            visitedScene.pickedUp |= pickUp;

            this.data.visitedScenes[i] = visitedScene;

            return;
        }
    }

    public int GetPickedUpForScene(SceneName scene)
    {
        var visitedScene = this.data.visitedScenes.Find(p => p.name == scene);

        if (visitedScene.name != scene) return 0;

        return visitedScene.pickedUp;
    }

    public bool HasVisitedScene(SceneName scene)
    {
        foreach (var visitedScene in this.data.visitedScenes)
        {
            if (scene == visitedScene.name) return true;
        }

        return false;
    }

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + this.saveName;

        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, this.data);

        File.WriteAllText(path + ".json", JsonUtility.ToJson(this.data));

        Debug.Log("Saving");

        stream.Close();
    }

    public void Load(bool loadOverride = false)
    {
        string path = Application.persistentDataPath + "/" + this.saveName;

        if(File.Exists(path) && (this.loadFromFile || loadOverride))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            this.data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();
        }
        else
        {
            this.data = new PlayerData();
            this.data.currentScene = this.defaultScene;
        }

        if (this.onLoad != null)
        {
            this.onLoad(this.data);
        }
    }

}
