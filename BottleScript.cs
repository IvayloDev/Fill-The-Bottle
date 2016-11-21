using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BottleScript : MonoBehaviour {

    public int minAmount, maxAmount;

    private Rigidbody2D rb;

    [HideInInspector]
    public Image fillment;

    [HideInInspector]
    public float bottleFillAmount = 0;

    public Vector3 SlideVector;

    [HideInInspector]
    public Animator bottleFillmentAnim, bottleAnim;

    void Awake() {
        fillment = GetComponentsInChildren<Image>()[1];
        bottleFillmentAnim = GetComponentsInChildren<Animator>()[1];
        bottleAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    //Setup
    void Start() {
        fillment.color = new Color32(255, 255, 255, 0);
        rb.isKinematic = true;
    }

    void Update() {

        fillment.fillAmount = bottleFillAmount;

    }

    public void Fill(float amount, int speed) {

        bottleFillAmount += (amount * speed) * Time.deltaTime;

    }

    public void KickBottle() {

        StartCoroutine(ResetBottlePos());

        rb.isKinematic = false;

        rb.AddForceAtPosition(new Vector2(Random.Range(-10, 10),
          Random.Range(-10, 10)), new Vector2(5, Random.Range(-15, -25)));



    }

    IEnumerator ResetBottlePos() {

        GameManager.resetingGame = true;

        yield return new WaitForSeconds(1.2f);

        GetComponent<Image>().enabled = false;
        fillment.enabled = false;

        rb.isKinematic = true;

        yield return new WaitForSeconds(0.1f);

        transform.localPosition = SlideVector;

        transform.rotation = new Quaternion(0, 0, 0, 0);

        yield return new WaitForSeconds(0.1f);

        GetComponent<Image>().enabled = true;
        fillment.enabled = true;

        bottleAnim.SetTrigger("Slide");

        GameManager.resetingGame = false;

    }

    public void ResetBottle() {

        bottleFillAmount = 0;
        bottleFillmentAnim.SetBool("FadeAlpha", false);
        fillment.color = new Color32(255, 255, 255, 0);


    }


    public void FillAlpha() {
        bottleFillmentAnim.SetBool("FadeAlpha", true);
    }

}
