using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enemy_controller : MonoBehaviour
{
    private Transform player;
    private Transform eyeLocation;
    private int health;
    private TextMeshPro healthValueText;

    protected float visionRange;
    protected float hearingRange;
    protected Vector2 startPos;
    protected float moveSpeed;
    protected bool isFacingLeft;
    protected float movementDistance;

    protected Rigidbody2D rb2d;

    // Update is called once per frame
    protected void UpdateCall(){
        if(Vision(visionRange, "Player")){
            ChasePlayer();
        } else{
            if(Hearing(hearingRange, "Player")){
                ChasePlayer();
            } else {
                StopChasingPlayer();
            }
        }
        healthValueText.text = health.ToString();
    }

    // All enemy start with this
    protected void StartInit(float vr, float hr, float ms, float md, int h){
        player = GameObject.Find("Player").GetComponent<Transform>();
        eyeLocation = this.gameObject.transform.GetChild(0).GetComponent<Transform>();
        healthValueText = this.gameObject.transform.GetChild(1).GetComponent<TMPro.TextMeshPro>();
        healthValueText.text = health.ToString();
        rb2d = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        visionRange = vr;
        hearingRange = hr;
        moveSpeed = ms;
        movementDistance = md;
        health = h;
        if(transform.localScale.x == 1){
            isFacingLeft = true;
        } else{
            isFacingLeft = false;
        }
    }

    protected bool Vision(float distance, string layer) {
        bool val = false;
        var castDist = distance;

        if(isFacingLeft) {
            castDist = -distance;
        }
        
        Vector2 endPos = eyeLocation.position + Vector3.right * castDist;
        RaycastHit2D hit = Physics2D.Linecast(eyeLocation.position, endPos, 1 << LayerMask.NameToLayer(layer));
        if(hit.collider != null){
            RaycastHit2D hitTwo = Physics2D.Linecast(eyeLocation.position, player.transform.position, 1 << LayerMask.NameToLayer("Ground"));

                if(hitTwo.collider == null){
                    val = true;
                    Debug.DrawLine(eyeLocation.position, hit.point, Color.green);
                } else{
                    Debug.DrawLine(eyeLocation.position, endPos, Color.red);
                }
        }
        return val;
    }

    protected bool Hearing(float distance, string gameObjectName) {
        bool val = false;
        Transform got = GameObject.Find(gameObjectName).transform;
        
        var objectDistance = (got.position - transform.position).magnitude;
        
        if(objectDistance < distance) {
            val = true;
        }

        return val;
    }

    protected void ChasePlayer(){
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

    protected void StopChasingPlayer(){
        rb2d.velocity = Vector2.zero;
    }
}
