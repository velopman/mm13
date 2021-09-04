using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] List<GameObject> connectedDoors;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            // GameObject door = transform.parent.transform.Find("Blockage").gameObject;
            // if(door != null) {
            //     door.GetComponent<BoxCollider2D>().enabled = false;
            //     transform.GetComponent<SpriteRenderer>().enabled = false;
            // }
            // foreach(Transform child in transform.parent){
            //     if(child.tag == "Door"){
            //         child.GetComponent<BoxCollider2D>().enabled = false;
            //         transform.GetComponent<SpriteRenderer>().enabled = false;
            //     }
            // }
            foreach(GameObject door in connectedDoors){
                door.SendMessage("buttonActive", 0);
            }
            transform.GetComponent<SpriteRenderer>().enabled = false;        
        }
    }
}
