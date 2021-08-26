using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlay : MonoBehaviour
{
    private HealthSystem healthSystem;
    private TextMeshProUGUI healthValueText;
    private int maxHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        healthSystem = new HealthSystem(maxHealth);
        healthValueText = returnText("HealthValue");
    }

    // Update is called once per frame
    void Update(){
        healthValueText.text = healthSystem.getHealth().ToString();
    }

    protected TextMeshProUGUI returnText(string name){
        return GameObject.Find(name).GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void DealDamage(int damage){
        healthSystem.SetHealth(-damage);
    }
}
