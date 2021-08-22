using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_ground : enemy_controller
{
    void Update()
    {
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
