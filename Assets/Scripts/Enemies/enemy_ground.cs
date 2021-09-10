using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_ground : enemy_controller
{
    private bool goingRight;
    private Vector2 walkForce;

    void Start(){
        StartInit(7f, 5f, 3f, 1f, 40);
        walkForce = new Vector2(30f, 0);
        goingRight = true;
    }

    void FixedUpdate(){
        Movement();
        UpdateCall();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Collider2D thisCol = other.contacts[0].collider;
        if(other.gameObject.tag == "Ground" || thisCol.gameObject.name == "Head"){
            goingRight = !goingRight;
            isFacingLeft = !isFacingLeft;
        }
        if(other.gameObject.tag == "Player"){
            GameObject.Find("UI").SendMessage("DealDamage", 20);
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
