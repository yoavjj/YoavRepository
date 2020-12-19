using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    public static CardManager instance;

    public List<Sprite> spriteList = new List<Sprite>();

    [SerializeField]private List<GameObject> buttonList = new List<GameObject>();
    [SerializeField]private List<GameObject> hiddenButtonList = new List<GameObject>();

    private List<GameObject> choosenCards = new List<GameObject>();

    private int lastMatchId;
    [SerializeField]private bool choosen;

    [Header("How many pairs you want to play with?")]
    public int pairs;

    [Header("Card Prefab Button")]
    public GameObject cardPrefab;

    [Header("The Parent Spacer to sort the Cards in")]
    public Transform spacer;
    // Particle FX

    [Header("Basic Score per Match")]
    public int matchScore = 100;

    public int choise1;
    public int choise2;

    [SerializeField] float sneekPick = 1.5f;

    [Header("Effects")]
    public GameObject fxExplosion;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FillPlayField();
    }

    void FillPlayField()
    {
        for (int i = 0; i < (pairs * 2); i++)
        {
            GameObject newCard = Instantiate(cardPrefab, spacer);
            buttonList.Add(newCard);
            hiddenButtonList.Add(newCard);
        }
        ShuffleCards();
    }

    void ShuffleCards()
    {
        int num = 0;
        int cardPairs = buttonList.Count / 2;

        for(int i = 0; i < cardPairs; i++)
        {
            num++;
            for(int j = 0; j < 2; j++)//Count card amount per match 
            {
                int cardIndex = Random.Range(0, buttonList.Count);
                Card tempCard = buttonList[cardIndex].GetComponent<Card>();
                tempCard.id = num;
                tempCard.cardFront = spriteList[num - 1];

                buttonList.Remove(buttonList[cardIndex]);
            }
        }
    }

    public void AddChoosenCard(GameObject card)
    {
        choosenCards.Add(card);
    }

    public IEnumerator CompareCards()
    {
        if(choise2 == 0 || choosen)
        {
            yield break;
        }
        choosen = true;

        //No match
        if((choise1 != 0 && choise2 != 0) && (choise1 != choise2))
        {
            //flip back the open cards 
            yield return new WaitForSeconds(sneekPick);
            FlipAllBack();

            //reset the combo in score  manager
            ScoreManager.instance.ResetCombos();
        }
        else if ((choise1 != 0 && choise2 != 0) && (choise1 == choise2))
        {
            foreach (var item in choosenCards)
            {
                item.GetComponent<Card>().success();
            }
            lastMatchId = choise1;
            //Add score 
            ScoreManager.instance.AddScore(matchScore);
            //Remove the match 
            RemoveMatch();

            // Remover choosen cards
            choosenCards.Clear();
        }
        //Reset all choises
        choise1 = 0;
        choise2 = 0;
        choosen = false;

        //Check if won
        CheckWin();
    }

    void FlipAllBack()
    {
        foreach (GameObject card in choosenCards)
        {
            card.GetComponent<Card>().CloseCard();
        }
        choosenCards.Clear();
    }

    void RemoveMatch()
    {
        for (int i = hiddenButtonList.Count - 1; i >= 0; i--) //-1 or will have overflow
        {
            Card tempCard = hiddenButtonList[i].GetComponent<Card>();
            if(tempCard.id == lastMatchId)
            {
                //Particle FX 
                Instantiate(fxExplosion, hiddenButtonList[i].transform.position + new Vector3(0,0,-1),Quaternion.identity);
                //Remove the card 
                hiddenButtonList.RemoveAt(i);

                //Add rewards to tooltip
            }
        }
    }

    void CheckWin()
    {
        if(hiddenButtonList.Count < 1)
        {
            //Stop Time
            ScoreManager.instance.stopTime();
            //Show UI

            // Open new popup with rewards 

            Debug.Log("You won");
        }
    }
}
