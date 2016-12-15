using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Text scoreTxt, coinTxt, HighScoreTxt;
    [HideInInspector]
    public int Score;
    [HideInInspector]
    public int HighScore;
    [HideInInspector]
    public int Coins;

    private int pourSpeed;

    public static bool fingerReleased, resetingGame;

    private BottleScript bottle;
    private DispenserScript dispenser;

    public GameObject goalFillLine, PauseGO, PourAnimGO;
    private GameObject currentFillLine;
    // Panels
    public GameObject pauseOverlayPanel, endScreenOverlayPanel, HighScoreTxtGO, Shop;
    public Image secondSpeed, thirdSpeed;
    public Text CoinsShop;
    // Animators
    public Animator ScoreAnimator, goalFillLineAnim, RestartAnim,
        ShopAnim, ClickToStartAnim, DispenserAnim;

    float curFillLineAmount;

    void Awake() {

        Coins = PlayerPrefs.GetInt("Coins");
        HighScore = PlayerPrefs.GetInt("HighScore");

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

        currentFillLine = GameObject.Find("FillmentLine");

        if (currentFillLine == null) {
            return;
        }

        PourAnimGO.SetActive(false);

        PauseGO.SetActive(true);
        HighScoreTxtGO.SetActive(false);
        endScreenOverlayPanel.SetActive(false);
        pauseOverlayPanel.SetActive(false);

        goalFillLineAnim.SetBool("LineSlideOut", true);

        PourAnimGO.GetComponent<Image>().color = FindObjectOfType<BottleScript>().liquidColor;

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
                secondSpeed.enabled = false;
                thirdSpeed.enabled = false;
                break;
            case 2:
                secondSpeed.enabled = true;
                thirdSpeed.enabled = false;
                break;
            case 3:
                secondSpeed.enabled = true;
                thirdSpeed.enabled = true;
                break;

        }
    }

    public void Reward(int score, int coin) {

        Score += score;
        ScoreAnimator.SetTrigger("ScorePop");
        //(LATER) If exactly on line >> give 5 points else 2

        Coins += coin;

    }
    public void Reward(int score) {

        Score = 0;

    }

    void Update() {

        ManageInput();

        HighScoreTxt.text = HighScore.ToString();
        scoreTxt.text = Score.ToString();

        coinTxt.text = Coins.ToString();
        CoinsShop.text = Coins.ToString();

        //If players' score get bigger than the highscore.

        if (Score >= HighScore) {
            HighScore = Score;
        }


        if (Score == 0) {
            scoreTxt.enabled = false;
        } else {
            scoreTxt.enabled = true;
        }

        if (bottle == null) {
            return;
        }
        //Keep GameObject with attached BoxCollider2D on the max fill value
        curFillLineAmount = bottle.fillment.GetComponent<RectTransform>().sizeDelta.y * bottle.fillment.fillAmount;
        currentFillLine.transform.localPosition = new Vector3(0, curFillLineAmount, 0);
    }

    void ManageInput() {

        if (Shop.activeSelf) {
            return;
        }

        if (Input.GetMouseButton(0) && !fingerReleased && !resetingGame) {
            // Player is holding the screen --> fill

            bottle.Fill(0.2f, pourSpeed);
            dispenser.Dispense(0.2f, pourSpeed);

            ClickToStartAnim.SetTrigger("Fade");
            DispenserAnim.SetBool("DispenserUp", false);
            DispenserAnim.SetBool("DispenserDown", true);
            PourAnimGO.SetActive(true);

        }

        if (Input.GetMouseButtonUp(0)) {

            fingerReleased = true;
            PourAnimGO.SetActive(false);


        }
    }

    public void OnApplicationPause(bool pause) {

        if (pause) {
            PlayerPrefs.SetInt("Coins", Coins);
            PlayerPrefs.SetInt("HighScore", HighScore);
        }
    }

    public void OnApplicationQuit() {

        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.SetInt("HighScore", HighScore);

    }




    // UI BUTTONS

    public void OnShopOpen() {

        Shop.SetActive(true);

    }

    public void OnShopClose() {

        Shop.SetActive(false);

    }

    public void OnPauseClick() {
        Time.timeScale = 0;
        pauseOverlayPanel.SetActive(true);
    }

    public void OnResumeClick() {
        Time.timeScale = 1;
        pauseOverlayPanel.SetActive(false);
    }

    public void OnRestartClick() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
