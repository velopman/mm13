using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_ground : enemy_controller
{
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        visionRange = 5f;
        hearingRange = 3f;
        moveSpeed = 2f;
        if(transform.localScale.x == 1){
            isFacingLeft = true;
        } else{
            isFacingLeft = false;
        }
    }

    void Update(){
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
}
