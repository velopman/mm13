using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] int secondsInactive;
    SpriteRenderer sr;
    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite inactiveSprite;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = activeSprite;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            if(secondsInactive > 0){
                StartCoroutine(ReActivate(secondsInactive));
            }
            sr.sprite = inactiveSprite;      
            this.transform.gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }            

    IEnumerator ReActivate(int seconds){
        yield return new WaitForSeconds(seconds);
        this.transform.gameObject.GetComponent<Collider2D>().enabled = true;
        sr.sprite = activeSprite; 
    }

}
