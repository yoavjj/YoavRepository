using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    //Score 
    public int currentScore;
    //Combo / Turn
    private int currentComboAmount;
    private int currentTurn;
    //Playtime 
    public int playtime;
    private int seconds;
    private int minutes;
    [Header("Text Connections")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comoboText;
    public TextMeshProUGUI turnsText;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        updateScoreText();
        StartCoroutine("playTime");
        
    }


    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int scoreAmount)
    {
        currentComboAmount++;
        currentTurn++;
        currentScore += scoreAmount * currentComboAmount;
        updateScoreText();
    }

    public void ResetCombos()
    {
        currentComboAmount = 0;
        currentTurn++;
        updateScoreText();
    }

    void updateScoreText()
    {
        scoreText.text = "score: " + currentScore.ToString("N");
        comoboText.text = "Combo: " + currentComboAmount.ToString();
        turnsText.text = currentTurn.ToString();
    }

    IEnumerator playTime()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            playtime++;
            seconds = (playtime % 60);
            minutes = (playtime / 60) % 60;
            UpdateTime();
        }
    }

    void UpdateTime()
    {
        timeText.text = "Time: " + minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    public void stopTime()
    {
        StopCoroutine("playTime");
    }

}
