using UnityEngine;
using TMPro;

public class SingletonScoreManager : MonoBehaviour
{
    public static SingletonScoreManager Instance { get; private set; }
    private TMP_Text scoreText;
    public GameObject scoreCanvas;

    public enum ScoreOperation { Increase, Decrease, Multiply, Divide }

    private int currentScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetScore();
        AssignScoreText();
        UpdateScoreText();
    }

    private void OnEnable()
    {
        AssignScoreText();
        UpdateScoreText();
    }

    public void IncreaseScore(int amount)
    {
        currentScore += amount;
        UpdateScoreText();
    }

    public void DecreaseScore(int amount)
    {
        currentScore -= amount;
        UpdateScoreText();
    }

    public void MultiplyScore(int amount)
    {
        currentScore *= amount;
        UpdateScoreText();
    }

    public void DivideScore(int amount)
    {
        if (amount != 0)
        {
            currentScore /= amount;
        }
        UpdateScoreText();
    }

    private void AssignScoreText()
    {
        scoreCanvas = GameObject.FindWithTag("ScoreCanvas");
        if (scoreCanvas != null)
        {
            scoreText = scoreCanvas.GetComponentInChildren<TMP_Text>();
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreText();
    }
}
