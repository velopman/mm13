using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_ground : enemy_controller
{
    private bool goingRight;
    private Vector2 walkForce;

    void Start(){
        StartInit(7f, 5f, 3f, 1f);
        walkForce = new Vector2(30f, 0);
        goingRight = true;
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

    private void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("Test");
        Collider2D thisCol = col.contacts[0].collider;
        if(col.gameObject.tag == "Ground" || thisCol.gameObject.name == "Head"){
            Debug.Log("Test2");
            goingRight = !goingRight;
            isFacingLeft = !isFacingLeft;
        }
    }

    private void Movement(){
        var distanceChange = startPos.x - transform.position.x;
        if(distanceChange > movementDistance){
            goingRight = true;
        } else if(distanceChange < -movementDistance){
            goingRight = false;
        }

        if(goingRight || !isFacingLeft){
            rb2d.AddForce(walkForce, ForceMode2D.Force);
            transform.localScale = new Vector2(-1,1);
            isFacingLeft = false;
        } else{
            rb2d.AddForce(-walkForce, ForceMode2D.Force);
            transform.localScale = new Vector2(1,1);
            isFacingLeft = true;
        }
    }
}
