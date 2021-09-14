using UnityEngine;

public class PickUpPoint : PickUp
{
    // Lifecylce methods

    void OnTriggerEnter2D(Collider2D collider)
    {
        var score = collider.GetComponent<PlayerScoreController>();
        if (score == null) return;

        score.AddScore(10);

        Destroy(this.gameObject);
    }
}
