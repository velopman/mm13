using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockage : MonoBehaviour
{
    [SerializeField] List<GameObject> connectedButtons;
    // 1 = active, 0 = not active
    private List<int> activeButtons;

    private void Start() {
        if(activeButtons != null) {
            foreach(GameObject button in connectedButtons){
                activeButtons.Add(0);
            }
        }
    }

    private void buttonActive(int index){
        activeButtons[index] = 1;
    }

    private void Update() {
        if(activeButtons != null) {
            if(activeButtons.Contains(0)){
                
            }else{
                transform.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

    }
}
