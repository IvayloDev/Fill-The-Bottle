using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Text scoreTxt, coinTxt;
    private int Score;
    private int HighScore;
    private int Coins;

    private int pourSpeed;

    public static bool fingerReleased, resetingGame;

    private BottleScript bottle;
    private DispenserScript dispenser;

    public GameObject goalFillLine, currentFillLine;
    public Image secondSpeed, thirdSpeed;

    float curFillLineAmount;

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

        StartGame();

    }

    public void StartGame() {

        // Set random position for the fill line the user has to reach.
        goalFillLine.transform.localPosition = new Vector3(-80, Random.Range(bottle.minAmount, bottle.maxAmount), 0);

        //Get random dispense speed.
        SetDispenseSpeed();


        fingerReleased = false;

    }

    void SetDispenseSpeed() {

        pourSpeed = Random.Range(1, 4);

        switch (pourSpeed) {
            case 1:
                secondSpeed.color = new Color32(255, 255, 255, 140);
                thirdSpeed.color = new Color32(255, 255, 255, 140);
                break;
            case 2:
                secondSpeed.color = new Color32(255, 255, 255, 255);
                thirdSpeed.color = new Color32(255, 255, 255, 140);
                break;
            case 3:
                secondSpeed.color = new Color32(255, 255, 255, 255);
                thirdSpeed.color = new Color32(255, 255, 255, 255);
                break;

        }
    }

    public void Reward(int score, int coin) {

        Score += score;

        //(LATER) If exactly on line >> give 5 points else 2

        Coins += coin;

    }
    public void Reward(int score) {

        Score = 0;

    }

    void Update() {

        ManageInput();

        scoreTxt.text = Score.ToString();
        coinTxt.text = Coins.ToString();

        //Keep GameObject with attached BoxCollider2D on the max fill value
        curFillLineAmount = bottle.fillment.GetComponent<RectTransform>().sizeDelta.y * bottle.fillment.fillAmount;
        currentFillLine.transform.localPosition = new Vector3(0, curFillLineAmount, 0);

    }

    void ManageInput() {

        if (Input.GetMouseButton(0) && !fingerReleased && !resetingGame) {
            // Player is holding the screen --> fill

            bottle.Fill(0.2f, pourSpeed);
            dispenser.Dispense(0.2f, pourSpeed);


        }

        if (Input.GetMouseButtonUp(0)) {

            fingerReleased = true;

        }
    }

}
