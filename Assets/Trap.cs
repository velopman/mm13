using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite inactiveSprite;
    [SerializeField] int secondsToActive;

    private int secondsToInactive;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        secondsToInactive = 2;
        sr.sprite = inactiveSprite;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            StartCoroutine(Activate(secondsToActive));
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            if(sr.sprite == activeSprite){
                var health = other.gameObject.GetComponent<HealthController>();
                health.damage(10, false);
                this.transform.gameObject.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    IEnumerator Activate(int seconds){
        yield return new WaitForSeconds(seconds);
        sr.sprite = activeSprite;
        StartCoroutine(ReActivate(seconds));
    }

    IEnumerator ReActivate(int seconds){
        yield return new WaitForSeconds(secondsToInactive);
        this.transform.gameObject.GetComponent<Collider2D>().enabled = true;
        sr.sprite = inactiveSprite;
    }
}
