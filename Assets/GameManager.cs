using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    private int score;
    public TextMeshProUGUI scoreText;
    private void Awake()
    {
        instance = this; 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetScoreText(int scoreValue)
    {
        score += scoreValue;
        Debug.Log(score);
        scoreText.text = score.ToString();
    }    
}
