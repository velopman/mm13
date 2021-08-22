using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_flying : enemy_controller {
    private float movementDistance;
    private Vector2 flyForce;
    private Vector2 fallForce;
    private bool goingDown;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        visionRange = 5f;
        hearingRange = 3f;
        moveSpeed = 3f;
        movementDistance = 1f;
        goingDown = true;
        flyForce = new Vector2(0,30.0f);
        fallForce = new Vector2(0,-10.0f);
        if(transform.localScale.x == 1){
            isFacingLeft = true;
        } else{
            isFacingLeft = false;
        }
    }

    void FixedUpdate(){
        Movement();
        if(Vision(visionRange, "Player")){
            ChasePlayer();
        } else{
            if(Hearing(hearingRange, "Player")){
                ChasePlayer();
            } else {
                StopChasingPlayer();
            }
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
}
