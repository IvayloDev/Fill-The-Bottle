using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public Text scoreTxt, coinTxt, HighScoreTxt;
    [HideInInspector]
    public int Score;
    [HideInInspector]
    public int HighScore;
    [HideInInspector]
    public int Coins;

    [HideInInspector]
    public static int adShownCounter;
    private int pourSpeed;

    public static bool fingerReleased, resetingGame;

    private BottleScript bottle;
    private DispenserScript dispenser;
    public AudioSource pourAudio;

    public GameObject goalFillLine, PourAnimGO;
    private GameObject currentFillLine;
    // Panels
    public GameObject endScreenOverlayPanel, HighScoreTxtGO, Shop;
    public Image secondSpeed, thirdSpeed;
    public Text CoinsShop;
    public GameObject WatchAdButtonGO;
    // Animators
    public Animator ScoreAnimator, goalFillLineAnim, RestartAnim,
        ShopAnim, ClickToStartAnim, DispenserAnim;

    float curFillLineAmount;

    void Awake() {

        adShownCounter = PlayerPrefs.GetInt("adShown", 0);
        Coins = PlayerPrefs.GetInt("Coins");
        HighScore = PlayerPrefs.GetInt("HighScore");

        if (instance != null) {
            Debug.LogError("More than one GameManager in scene");
        } else {
            instance = this;
        }

        dispenser = FindObjectOfType<DispenserScript>();

    }

    void Start() {

        StartGame();
    }

    public void StartGame() {

        Shop.SetActive(false);

        bottle = FindObjectOfType<BottleScript>();

        currentFillLine = GameObject.Find("FillmentLine");

        PourAnimGO.SetActive(false);

        HighScoreTxtGO.SetActive(false);
        endScreenOverlayPanel.SetActive(false);

        goalFillLineAnim.SetBool("LineSlideOut", true);

        if (FindObjectOfType<BottleScript>() != null) {
            PourAnimGO.GetComponent<Image>().color = bottle.liquidColor;

            // Set random position for the fill line the user has to reach.

            goalFillLine.transform.localPosition = new Vector3(-100, Random.Range(bottle.minAmount, bottle.maxAmount), 0);
        }

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

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        if (adShownCounter >= 3 && adShownCounter <= 4) {
            WatchAdButtonGO.SetActive(false);
        }

        if (adShownCounter > 5) {
            WatchAdButtonGO.SetActive(true);
            adShownCounter = 0;
        }

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

        if (bottle.fillment.fillAmount == 1) {
            fingerReleased = true;
            PourAnimGO.SetActive(false);
        }

        if (Shop.activeSelf) {
            return;
        }

        if (Input.GetMouseButton(0) && !fingerReleased && !resetingGame) {
            // Player is holding the screen --> fill

            bottle.Fill(0.2f, pourSpeed);
            dispenser.Dispense(0.2f, pourSpeed);

            pourAudio.enabled = true;

            ClickToStartAnim.SetTrigger("Fade");
            DispenserAnim.SetBool("DispenserUp", false);
            DispenserAnim.SetBool("DispenserDown", true);
            PourAnimGO.SetActive(true);

        }

        if (Input.GetMouseButtonUp(0)) {

            pourAudio.enabled = false;
            fingerReleased = true;
            PourAnimGO.SetActive(false);


        }
    }

    public void OnApplicationPause(bool pause) {

        if (pause) {
            PlayerPrefs.SetInt("Coins", Coins);
            PlayerPrefs.SetInt("HighScore", HighScore);
            PlayerPrefs.SetInt("adShown", adShownCounter);
        }
    }

    public void OnApplicationQuit() {

        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.SetInt("HighScore", HighScore);
        PlayerPrefs.SetInt("adShown", adShownCounter);
    }




    // UI BUTTONS

    public void OnWatchAdClick() {

        AudioManager.instance.PlaySound("ButtonClick");

        if (Advertisement.IsReady("rewardedVideo")) {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result) {
        switch (result) {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                Coins += 5;
                adShownCounter++;

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }


    public void OnShopOpen() {

        AudioManager.instance.PlaySound("ButtonClick");
        Shop.SetActive(true);

    }

    public void OnShopClose() {

        AudioManager.instance.PlaySound("ButtonClick");
        Shop.SetActive(false);

    }


    public void OnRestartClick() {

        AudioManager.instance.PlaySound("ButtonClick");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
