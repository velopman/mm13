using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_flying : enemy_controller {
    private Vector2 flyForce;
    private Vector2 fallForce;
    private bool goingDown;

    void Start(){
        StartInit(5f, 3f, 2f, 1f, 10);
        goingDown = true;
        flyForce = new Vector2(0,30.0f);
        fallForce = new Vector2(0,-10.0f);

        var health = this.GetComponent<HealthController>();
        health.onDeath += this.death;
    }


    void FixedUpdate(){
        Movement();
        UpdateCall();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player"){
            var health = other.gameObject.GetComponent<HealthController>();
            health.damage(1, this.dealLethal);
        }
    }

    private void Movement(){
        var distanceChange = startPos.y - transform.position.y;
        if(distanceChange > movementDistance){
            goingDown = false;
        } else if(distanceChange < -movementDistance){
            goingDown = true;
        }

        if(!goingDown){
            rb2d.AddForce(flyForce, ForceMode2D.Force);
        } else{
            rb2d.AddForce(fallForce, ForceMode2D.Force);
        }
    }

    // Callback methods
    void death(HealthController self, bool lethal)
    {
        Destroy(this.gameObject);
    }
}
