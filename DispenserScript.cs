using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DispenserScript : MonoBehaviour {

    public Image dispenser_Fillment;
    public float dispenserFill = 0;

    void Start() {


    }

    public void Dispense(float amount, int speed) {

        dispenserFill += (amount * speed) * Time.deltaTime;

    }

    void Update() {

        dispenser_Fillment.fillAmount = dispenserFill;

    }
}
