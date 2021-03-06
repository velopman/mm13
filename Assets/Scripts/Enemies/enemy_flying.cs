using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_flying : enemy_controller {
    [SerializeField] GameObject ps;
    
    private int flySpeed;
    private int flyForce;
    private bool goingDown;

    void Start(){
        StartInit(5f, 3f, 2f, 1f, 10);
        deathParticle = ps;
        goingDown = true;
        flySpeed = 2;
        flyForce = 1000;
    }


    void FixedUpdate(){
        Movement();
        UpdateCall();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player"){
            var health = other.gameObject.GetComponent<HealthController>();
            health.damage(1, this.dealLethal);
            var targetDir = other.gameObject.GetComponent<Rigidbody2D>().velocity;
            rb2d.AddForce(targetDir * flyForce, ForceMode2D.Force);
            goingDown = true;
        }
    }

    private void Movement(){
        var distanceChange = startPos.y - transform.position.y;
        if(distanceChange > movementDistance){
            goingDown = false;
        } else if(distanceChange < -movementDistance){
            goingDown = true;
        }

        if(goingDown){
            rb2d.velocity = new Vector2(rb2d.velocity.x, -flySpeed);        
        } else{
            rb2d.velocity = new Vector2(rb2d.velocity.x, flySpeed); 
        }
    }
}
