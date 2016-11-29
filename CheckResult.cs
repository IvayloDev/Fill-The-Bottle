using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CheckResult : MonoBehaviour {

    private bool checkGameBool;
    private bool gameFinishedBool;

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

        FindObjectOfType<GameManager>().StartGame();
        FindObjectOfType<BottleScript>().ResetBottle();
        FindObjectOfType<DispenserScript>().ResetDispenser();
        checkGameBool = false;
        gameFinishedBool = false;
        FindObjectOfType<BottleScript>().fillmentLineSlideUpAnim.SetBool("SlideCurrentLine", false);

    }

    public IEnumerator NextLevel() {

        FindObjectOfType<BottleScript>().fillmentLineSlideUpAnim.SetBool("SlideCurrentLine", true);
        FindObjectOfType<GameManager>().goalFillLineAnim.SetBool("LineSlideIn", true);

        yield return new WaitForSeconds(0.4f);

        FindObjectOfType<GameManager>().goalFillLineAnim.SetBool("LineSlideOut", false);

        yield return new WaitForSeconds(0.5f);

        FindObjectOfType<GameManager>().Reward(1, 2);

        FindObjectOfType<BottleScript>().KickBottle();

        ResetGame();
    }

    public IEnumerator GameOver() {

        FindObjectOfType<BottleScript>().fillmentLineSlideUpAnim.SetBool("SlideCurrentLine", true);

        FindObjectOfType<BottleScript>().FillAlpha();

        yield return new WaitForSeconds(0.5f);

        FindObjectOfType<BottleScript>().bottleAnim.SetBool("Cap", true);

        yield return new WaitForSeconds(0.4f);

        // END SCREEN panel and UI animations
        FindObjectOfType<GameManager>().endScreenOverlayPanel.SetActive(true);
        FindObjectOfType<GameManager>().RestartAnim.SetTrigger("RotateRestartButton");
        FindObjectOfType<GameManager>().ShopAnim.SetTrigger("ScaleShopButton");

        FindObjectOfType<GameManager>().Reward(0);

    }


}
