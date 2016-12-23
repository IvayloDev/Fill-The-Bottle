using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CheckResult : MonoBehaviour {

    private bool checkGameBool;
    private bool gameFinishedBool;

    private GameManager gm;

    [HideInInspector]
    public bool overFilled;

    void Awake() {
        gm = FindObjectOfType<GameManager>();
    }

    void Update() {

        if (GameManager.fingerReleased && !gameFinishedBool) {

            StartCoroutine(CheckResults());

            gameFinishedBool = true;
        }
    }

    IEnumerator CheckResults() {

        yield return new WaitForSeconds(0.1f);

        if (!checkGameBool) {

            StartCoroutine(GameOver());
        }

    }

    // Checking if player won.
    void OnTriggerStay2D(Collider2D col) {

        // If the player lifted his finger
        if (GameManager.fingerReleased) {

            if (col.tag == "Line" && !checkGameBool) {

                StartCoroutine(NextLevel());

                checkGameBool = true;

            }
        }
    }

    public void ResetGame() {

        gm.StartGame();

        FindObjectOfType<BottleScript>().ResetBottle();
        FindObjectOfType<DispenserScript>().ResetDispenser();
        checkGameBool = false;
        gameFinishedBool = false;
        FindObjectOfType<BottleScript>().fillmentLineSlideUpAnim.SetBool("SlideCurrentLine", false);


    }

    public IEnumerator NextLevel() {

        AudioManager.instance.PlaySound("Won");

        gm.DispenserAnim.SetBool("DispenserDown", false);
        gm.DispenserAnim.SetBool("DispenserUp", true);

        FindObjectOfType<BottleScript>().fillmentLineSlideUpAnim.SetBool("SlideCurrentLine", true);

        gm.goalFillLineAnim.SetBool("LineSlideIn", true);

        yield return new WaitForSeconds(0.4f);

        gm.goalFillLineAnim.SetBool("LineSlideOut", false);

        yield return new WaitForSeconds(0.5f);

        gm.Reward(1, 2);

        FindObjectOfType<BottleScript>().KickBottle();

        ResetGame();

    }

    public IEnumerator GameOver() {

        AudioManager.instance.PlaySound("Lost");

        gm.DispenserAnim.SetBool("DispenserDown", false);
        gm.DispenserAnim.SetBool("DispenserUp", true);

        FindObjectOfType<BottleScript>().fillmentLineSlideUpAnim.SetBool("SlideCurrentLine", true);

        FindObjectOfType<BottleScript>().FillAlpha();

        yield return new WaitForSeconds(0.5f);

        if (gm.Score != 0) {


            FindObjectOfType<BottleScript>().bottleAnim.SetBool(FindObjectOfType<BottleScript>().CapName
                + " Cap", true);
        }

        yield return new WaitForSeconds(0.4f);

        // END SCREEN panel and UI animations
        if (gm.Score == 0) {

            ResetGame();

        } else {

            PlayerPrefs.SetInt("HighScore", gm.HighScore);
            PlayerPrefs.SetInt("Coins", gm.Coins);

            gm.HighScoreTxtGO.SetActive(true);
            gm.endScreenOverlayPanel.SetActive(true);
            gm.RestartAnim.SetTrigger("RotateRestartButton");
            gm.ShopAnim.SetTrigger("ScaleShopButton");
            gm.Reward(0);

        }
    }
}
