using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BottleScript : MonoBehaviour {

    [HideInInspector]
    public Image fillment;
    public float bottleFill = 0;

    private Color32 alpha_fillment;

    void Awake() {

        fillment = GetComponentsInChildren<Image>()[1];

    }

    void Start() {

        fillment.color = new Color32(255, 255, 255, 0);

    }

    void Update() {

        fillment.fillAmount = bottleFill;

    }

    public void Fill(float amount, int speed) {

        bottleFill += (amount * speed) * Time.deltaTime;

    }

    void FillAlpha() {

    }

}
