using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void Update() {
        int countChild = 0;
        foreach(Transform child in transform){
            if(child.gameObject.activeInHierarchy){
                countChild++;
            }
        }
        if(countChild <=0){
            transform.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
