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

    void ResetGame() {

        FindObjectOfType<GameManager>().StartGame();
        FindObjectOfType<BottleScript>().ResetBottle();
        FindObjectOfType<DispenserScript>().ResetDispenser();
        checkGameBool = false;
        gameFinishedBool = false;

    }

    public IEnumerator NextLevel() {


        yield return new WaitForSeconds(0.5f);

        //Activate rigidbody on bottle and "Kick" it

        FindObjectOfType<GameManager>().Reward(1, 2);

        FindObjectOfType<BottleScript>().KickBottle();

        ResetGame();
    }

    public IEnumerator GameOver() {

        FindObjectOfType<BottleScript>().FillAlpha();

        yield return new WaitForSeconds(0.8f);

        ResetGame();

        FindObjectOfType<GameManager>().Reward(0);

    }


}
