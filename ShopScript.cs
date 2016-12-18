using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopScript : MonoBehaviour {

    public int price;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    public Sprite selectedSprite;

    [HideInInspector]
    public int bottleSavedState;
    [HideInInspector]
    public int lastSelectedBottleIndex, firstGameInt;

    [HideInInspector]
    public Image image;

    public static bool bottleChanged;
    private ShopController shopController;

    void Awake() {

        shopController = FindObjectOfType<ShopController>();
        image = GetComponent<Image>();

        lastSelectedBottleIndex = PlayerPrefs.GetInt(
            selectedSprite.name.ToString() + "select");

        bottleSavedState = PlayerPrefs.GetInt(selectedSprite.name.ToString());


    }

    IEnumerator SetDefaultBottle() {

        yield return new WaitForSeconds(0.05f);

        if (FindObjectOfType<ShopController>().firstGame && firstGameInt == 0) {
            if (image.sprite.name == "water2" || image.sprite.name == "Water") {


                Debug.Log("ShopScript Default setting2");

                image.sprite = selectedSprite;
                PlayerPrefs.SetInt(selectedSprite.name.ToString(), 1);
                PlayerPrefs.SetInt(selectedSprite.name.ToString() + "select", 1);
                PlayerPrefs.Save();
            }
        }
    }

    void Start() {

        StartCoroutine(SetDefaultBottle());


        if (bottleSavedState == 1) {
            image.sprite = unlockedSprite;
        }

        if (bottleSavedState == 0) {
            image.sprite = lockedSprite;
        }

        if (lastSelectedBottleIndex == 1) {
            image.sprite = selectedSprite;

            Destroy(shopController.currBottle);

            shopController.currBottle = (GameObject)Instantiate(Resources.Load(unlockedSprite.name.ToString()),
                       FindObjectOfType<ShopController>().BottleParentHolder.transform, false);

        }
    }


    public void SaveBottle() {

        if (image.sprite == lockedSprite && FindObjectOfType<GameManager>().Coins >= price) {

            FindObjectOfType<GameManager>().Coins -= price;

            Destroy(shopController.currBottle);

            shopController.currBottle = (GameObject)Instantiate(Resources.Load(unlockedSprite.name.ToString()),
                       FindObjectOfType<ShopController>().BottleParentHolder.transform, false);

            FindObjectOfType<DispenserScript>().ChangeDispenserLiquid();

            bottleChanged = true;

            image.sprite = unlockedSprite;

            PlayerPrefs.SetInt(selectedSprite.name.ToString(), 1);

            PlayerPrefs.SetInt(selectedSprite.name.ToString() + "select", 1);

            image.sprite = selectedSprite;

            PlayerPrefs.Save();
        }
        if (image.sprite == unlockedSprite) {

            Destroy(shopController.currBottle);

            shopController.currBottle = (GameObject)Instantiate(Resources.Load(unlockedSprite.name.ToString()),
                        FindObjectOfType<ShopController>().BottleParentHolder.transform, false);

            FindObjectOfType<DispenserScript>().ChangeDispenserLiquid();

            bottleChanged = true;

            image.sprite = unlockedSprite;

            PlayerPrefs.SetInt(selectedSprite.name.ToString(), 1);

            PlayerPrefs.SetInt(selectedSprite.name.ToString() + "select", 1);

            image.sprite = selectedSprite;

            PlayerPrefs.Save();

        } else {
            Debug.Log("NO MONEY");
        }
    }

    public void ResetSelectedBottle() {

        PlayerPrefs.DeleteKey(selectedSprite.name.ToString() + "select");

        bottleChanged = false;

        if (image.sprite == selectedSprite) {
            Destroy(shopController.currBottle);

            Debug.Log("Reseting");
            image.sprite = unlockedSprite;

        }

    }

    void Update() {

    }

}

