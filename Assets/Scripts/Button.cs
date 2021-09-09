using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] int secondsInactive;
    SpriteRenderer sr;
    Color activeColor;
    Color inactiveColor;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        activeColor = Color.green;
        sr.color = activeColor;
        inactiveColor = Color.red;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            if(secondsInactive > 0){
                StartCoroutine(ReActivate(secondsInactive));
            }
            this.transform.gameObject.GetComponent<Collider2D>().enabled = false;
            sr.color = inactiveColor;      
        }
    }

    IEnumerator ReActivate(int seconds){
        yield return new WaitForSeconds(seconds);
        this.transform.gameObject.GetComponent<Collider2D>().enabled = true;
        sr.color = activeColor; 
    }

}
