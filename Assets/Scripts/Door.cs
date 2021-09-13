using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite inactiveSprite;

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = inactiveSprite;
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
            sr.sprite = activeSprite;
        } else {
            transform.GetComponent<BoxCollider2D>().enabled = true;
            sr.sprite = inactiveSprite;
        }
    }
}
