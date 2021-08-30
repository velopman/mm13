using System.Collections;
using UnityEngine;


class PlayerAttackController : MonoBehaviour
{
    private PlayerInputController input;

    private Vector2[] meleeCheckLocations;


    // Lifecycle methods

    void Start()
    {
        this.input = this.GetComponent<PlayerInputController>();
    }

    void FixedUpdate()
    {
        this.attackMelee();
    }

    void OnDrawGizmos()
    {
        if (this.meleeCheckLocations == null) return;

        foreach (var checkLocation in this.meleeCheckLocations)
        {
            Gizmos.DrawWireSphere((Vector2)this.transform.position + checkLocation, .25f);
        }
    }

    // Private methods
    private void attackMelee()
    {
        if (!this.input.meleeTriggered) return;

        var health = this.GetComponent<PlayerHealthController>();
        health.damage(1);

        if(this.meleeCheckLocations != null) return;

        this.input.ResetMelee();

        StartCoroutine(this.meleeSwipe());
    }


    IEnumerator meleeSwipe()
    {
        this.meleeCheckLocations = new Vector2[4];

        for (float rotation = 0f; rotation < 90f; rotation += 9f)
        {
            var offset = Quaternion.Euler(0f, 0f, -this.input.facing * (45f + rotation)) * Vector2.up;

            for (int i = 0; i < this.meleeCheckLocations.Length; ++i)
            {
                this.meleeCheckLocations[i] = offset * .5f * (i + 1);

                var other = Physics2D.OverlapCircle((Vector2)this.transform.position + this.meleeCheckLocations[i], .25f);
                if (!other) continue;

                var health = other.GetComponent<HealthController>();
            }

            yield return new WaitForSeconds(.025f);
        }

        this.meleeCheckLocations = null;
    }
}
