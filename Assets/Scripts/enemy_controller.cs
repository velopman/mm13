using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_controller : MonoBehaviour
{
    [SerializeField]Transform player;

    float agroRange;
    float moveSpeed;

    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        agroRange = 5f;
        moveSpeed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        //distance to player
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        // print("distToPlayer " + distToPlayer);
        if(distToPlayer < agroRange){
            // Chase player
            ChasePlayer();
        } else {
            // Stop chasing player
            StopChasingPlayer();
        }
    }

    private void ChasePlayer(){
        // Will change this to RayCast instead! 
        if(transform.position.x < player.position.x){
            // enemy to the left of player
            rb2d.velocity = new Vector2(moveSpeed, 0);
            transform.localScale = new Vector2(-1,1);
        } else{
            // enemy to the right of player (or on top of it)
            rb2d.velocity = new Vector2(-moveSpeed,0);
            transform.localScale = new Vector2(1,1);
        }
    }

    private void StopChasingPlayer(){
        rb2d.velocity = Vector2.zero;
    }
}
