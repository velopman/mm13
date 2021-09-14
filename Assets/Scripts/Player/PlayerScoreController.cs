using UnityEngine;

public class PlayerScoreController : MonoBehaviour
{
    public delegate void OnScoreUpdate(int score);
    public OnScoreUpdate onScoreUpdate;

    private PlayerSaveDataController data;
    private int score;


    // Lifecycle methods

    void Awake()
    {
        this.data = this.GetComponent<PlayerSaveDataController>();
        this.data.onLoad += this.onLoad;
    }


    // Public methods

    public void AddScore(int amount = 1)
    {
        this.score += amount;

        this.data.SetScore(this.score);

        if (this.onScoreUpdate != null)
        {
            this.onScoreUpdate(this.score);
        }
    }


    // Callback methods

    void onLoad(PlayerSaveDataController.PlayerData data)
    {
        this.score = data.score;

        if (this.onScoreUpdate != null)
        {
            this.onScoreUpdate(this.score);
        }
    }
}
