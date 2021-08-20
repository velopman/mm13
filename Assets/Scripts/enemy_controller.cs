using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_controller : MonoBehaviour
{
    [SerializeField]Transform player;
    [SerializeField]Transform eyeLocation;

    float visionRange;
    float hearingRange;
    float moveSpeed;
    bool isFacingLeft;

    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        visionRange = 5f;
        hearingRange = 3f;
        moveSpeed = 2f;
        if(transform.localScale.x == 1){
            isFacingLeft = true;
        } else{
            isFacingLeft = false;
        }
    }

    // Update is called once per frame
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

    bool Vision(float distance, string layer) {
        bool val = false;
        var castDist = distance;

        if(isFacingLeft) {
            castDist = -distance;
        }
        
        Vector2 endPos = eyeLocation.position + Vector3.right * castDist;
        RaycastHit2D hit = Physics2D.Linecast(eyeLocation.position, endPos, 1 << LayerMask.NameToLayer(layer));

        if(hit.collider != null){
            val = true;
            Debug.DrawLine(eyeLocation.position, hit.point, Color.green);
        } else{
            Debug.DrawLine(eyeLocation.position, endPos, Color.red);
        }
        return val;
    }

    bool Hearing(float distance, string gameObjectName) {
        bool val = false;
        Transform got = GameObject.Find(gameObjectName).transform;
        
        var objectDistance = (got.position - transform.position).magnitude;
        
        if(objectDistance < distance) {
            val = true;
        }

        return val;
    }

    private void ChasePlayer(){
        // Will change this to RayCast instead! 
        if(transform.position.x < player.position.x){
            // enemy to the left of player
            rb2d.velocity = new Vector2(moveSpeed, 0);
            transform.localScale = new Vector2(-1,1);
            isFacingLeft = false;
        } else{
            // enemy to the right of player (or on top of it)
            rb2d.velocity = new Vector2(-moveSpeed,0);
            transform.localScale = new Vector2(1,1);
            isFacingLeft = true;
        }
    }

    private void StopChasingPlayer(){
        rb2d.velocity = Vector2.zero;
    }
}
