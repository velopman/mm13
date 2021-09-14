using UnityEngine;

public class PickUpPoint : PickUp
{
    // Lifecylce methods

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        var score = collider.GetComponent<PlayerScoreController>();
        if (score == null) return;

        score.AddScore(10);

        base.OnTriggerEnter2D(collider);
    }
}
