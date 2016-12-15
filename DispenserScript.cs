using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DispenserScript : MonoBehaviour {

    public Image dispenser_Fillment;

    [HideInInspector]
    public float dispenserFill = 0;

    void Start() {
        if (FindObjectOfType<BottleScript>() == null) {
            return;
        }
        GetComponent<Image>().color = FindObjectOfType<BottleScript>().liquidColor;
    }

    public void Dispense(float amount, int speed) {

        dispenserFill += (amount * speed) * Time.deltaTime;

    }

    public void ResetDispenser() {
        dispenserFill = 0;
    }

    void Update() {
        dispenser_Fillment.fillAmount = dispenserFill;

    }
}
