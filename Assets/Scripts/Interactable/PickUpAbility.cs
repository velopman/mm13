using UnityEngine;

public class PickUpAbility : MonoBehaviour
{
    public PlayerAbility type;


    // Lifecylce methods

    void OnTriggerEnter2D(Collider2D collider)
    {
        var ability = collider.GetComponent<PlayerAbilityController>();
        if (ability == null) return;

        ability.UnlockAbility(this.type);

        Destroy(this.gameObject);
    }
}
