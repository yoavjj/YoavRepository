using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public static Card instance;

    public int id;
    public Sprite cardBack;
    public Sprite cardFront;

    [SerializeField] private Image image;
    private Button button;

    private bool isFlippingOpen;
    private bool isFlippingClose;
    private bool flipped;//true == cardfront
    private float flipAmount = 1;

    public float flipSpeed = 4;
    [SerializeField] Animator goodMatch;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //image = GetComponentInChildren<Image>();
        button = GetComponent<Button>();
    }

    // OnCLick To Flip Card Open
    public void FlipCard()
    {
        if(CardManager.instance.choise1 == 0)
        {
            CardManager.instance.choise1 = id;
            CardManager.instance.AddChoosenCard(this.gameObject);

            isFlippingOpen = true;  
            StartCoroutine(FlipOpen());

            button.interactable = false;
        }
        else if (CardManager.instance.choise2 == 0)
        {
            CardManager.instance.choise2 = id;
            CardManager.instance.AddChoosenCard(this.gameObject);

            isFlippingOpen = true;
            StartCoroutine(FlipOpen());

            button.interactable = false;

            //Comper the cards
            StartCoroutine(CardManager.instance.CompareCards());
        }
    }

    //Open The Card Over TIme
    IEnumerator FlipOpen()
    {
        while (isFlippingOpen && flipAmount > 0)
        {
            flipAmount -= Time.deltaTime * flipSpeed;
            flipAmount = Mathf.Clamp01(flipAmount);
            transform.localScale = new Vector3(flipAmount, transform.localScale.y, transform.localScale.z);

            if(flipAmount <= 0)
            {
                image.sprite = cardFront;
                isFlippingOpen = false;
                isFlippingClose = true;
            }
            yield return null;
        }

        while (isFlippingClose && flipAmount < 1)
        {
            flipAmount += Time.deltaTime * flipSpeed;
            flipAmount = Mathf.Clamp01(flipAmount);
            transform.localScale = new Vector3(flipAmount, transform.localScale.y, transform.localScale.z);

            if (flipAmount >= 1)
            {
                isFlippingClose = false;
            }
            yield return null;
        }
    }

    //Close The Card Over Time 
    IEnumerator FlipClose()
    {
        while (isFlippingOpen && flipAmount > 0)
        {
            flipAmount -= Time.deltaTime * flipSpeed;
            flipAmount = Mathf.Clamp01(flipAmount);
            transform.localScale = new Vector3(flipAmount, transform.localScale.y, transform.localScale.z);

            if (flipAmount <= 0)
            {
                image.sprite = cardBack;
                isFlippingOpen = false;
                isFlippingClose = true;
            }
            yield return null;
        }

        while (isFlippingClose && flipAmount < 1)
        {
            flipAmount += Time.deltaTime * flipSpeed;
            flipAmount = Mathf.Clamp01(flipAmount);
            transform.localScale = new Vector3(flipAmount, transform.localScale.y, transform.localScale.z);

            if (flipAmount >= 1)
            {
                isFlippingClose = false;
            }
            yield return null;
        }

        button.interactable = true;
    }

    // Close Card
    public void CloseCard()
    {
        isFlippingOpen = true;
        StartCoroutine(FlipClose());
    }

    //good Match
    public void success()
    {
        goodMatch.SetTrigger("GoodMatch");
        Debug.Log("Now Playing");
    }
}
