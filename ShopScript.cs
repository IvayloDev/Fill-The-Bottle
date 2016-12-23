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
    public int lastSelectedBottleIndex;

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

    void Start() {

        if (image.sprite.name == "Water Bottle" && FindObjectOfType<ShopController>().firstGameInt == 0) {
            return;
        }

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
            PlayerPrefs.SetInt("Coins", FindObjectOfType<GameManager>().Coins);

            GameManager.adShownCounter++;

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

        }
    }

    public void ResetSelectedBottle() {

        PlayerPrefs.DeleteKey(selectedSprite.name.ToString() + "select");

        bottleChanged = false;

        if (image.sprite == selectedSprite) {
            Destroy(shopController.currBottle);

            image.sprite = unlockedSprite;

        }

    }

    void Update() {

    }

}

