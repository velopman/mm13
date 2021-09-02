using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            GameObject door = transform.parent.transform.Find("Blockage").gameObject;
            if(door != null) {
                door.GetComponent<BoxCollider2D>().enabled = false;
                transform.GetComponent<SpriteRenderer>().enabled = false;
            }        
        }
    }
}
