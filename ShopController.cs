using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {

    [HideInInspector]
    public bool firstGame;
    private bool allReady;
    public GameObject BottleParentHolder;

    [HideInInspector]
    public GameObject currBottle;

    public Animator CoinsAnim;

    public void OnButtonPress(int index) {


        if (GetComponentsInChildren<ShopScript>()[index - 1].price > FindObjectOfType<GameManager>().Coins
            && GetComponentsInChildren<ShopScript>()[index - 1].image.sprite == GetComponentsInChildren<ShopScript>()[index - 1].lockedSprite) {
            CoinsAnim.SetTrigger("Shake");
            return;
        }

        for (int i = 0; i < 12; i++) {

            GetComponentsInChildren<ShopScript>()[i].ResetSelectedBottle();
        }

        GetComponentsInChildren<ShopScript>()[index - 1].SaveBottle();
    }

    void Awake() {

        FindObjectOfType<ShopScript>().firstGameInt = PlayerPrefs.GetInt("FirstGame", 0);


        // SET LAST BOTTLE TO CURR BOTTLE ON START GAME

        for (int i = 0; i < 12; i++) {

            if (GetComponentsInChildren<ShopScript>()[i].bottleSavedState == 1) {

                return;

            } else {
                allReady = true;

            }
        }

        if (allReady) {
            firstGame = true;

            currBottle = (GameObject)Instantiate(Resources.Load(
              "Water"),
                 FindObjectOfType<ShopController>().BottleParentHolder.transform, false);

            PlayerPrefs.SetInt("FirstGame", 1);
            PlayerPrefs.Save();

        }
    }

    void Update() {

    }

}
