using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultSceneLoader : MonoBehaviour
{
    public SceneName defaultScene;


    // Lifecycle methods

    void Awake()
    {
        // SceneManager.LoadSceneAsync(this.defaultScene.ToString().ToLower(), LoadSceneMode.Additive);
    }
}
