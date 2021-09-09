using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    SpriteRenderer sr;
    Color activeColor;
    Color inactiveColor;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        activeColor = Color.green;
        inactiveColor = Color.red;
        sr.color = inactiveColor;
    }

    private void Update() {
        int countChild = 0;
        foreach(Transform child in transform){
            if(child.gameObject.GetComponent<Collider2D>().enabled){
                countChild++;
            }
        }
        if(countChild <=0){
            transform.GetComponent<BoxCollider2D>().enabled = false;
            sr.color = activeColor;
        } else {
            transform.GetComponent<BoxCollider2D>().enabled = true;
            sr.color = inactiveColor;
        }
    }
}
