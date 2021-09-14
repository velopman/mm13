using UnityEngine;

public class PickUpAbility : PickUp
{
    public PlayerAbility type;


    // Lifecylce methods

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        var ability = collider.GetComponent<PlayerAbilityController>();
        if (ability == null) return;

        ability.UnlockAbility(this.type);

        base.OnTriggerEnter2D(collider);
    }
}
