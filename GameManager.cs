using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public int Score;
    public int HighScore;

    private int dispenseSpeed;

    private bool fingerReleased;

    private BottleScript bottle;
    private DispenserScript dispenser;


    void Awake() {

        if (instance != null) {
            Debug.LogError("More than one GameManager in scene");
        } else {
            instance = this;
        }

        bottle = FindObjectOfType<BottleScript>();
        dispenser = FindObjectOfType<DispenserScript>();

    }

    void Start() {


    }

    void OnTriggerStay2D(Collider2D col) {


    }

    void Update() {

        ManageInput();

    }

    void ManageInput() {

        if (Input.GetMouseButton(0) && !fingerReleased) {
            // Player is holding the screen --> fill

            bottle.Fill(0.2f, 1);
            dispenser.Dispense(0.2f, 1);

        }

        if (Input.GetMouseButtonUp(0)) {

            fingerReleased = true;
            CheckResult();

        }
    }

    void CheckResult() {


    }


}
