//9
using UnityEngine;
using UnityEngine.UI;

public class updatescore : MonoBehaviour
{
    [SerializeField] private Text lastScoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text bankScoreText;
    [SerializeField] private Text fieldScoreText;
    [SerializeField] private Text userLevel;
    [SerializeField] private Text currentMultiplier;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }

        UpdateLastScoreText();
        UpdateHighScoreText();
        UpdateBankScoreText();
        UpdateFieldScoreText();
        UpdateUserLevelText();
        UpdateUserMultiplierText();
    }

    private void Update()
    {
        UpdateLastScoreText();
        UpdateHighScoreText();
        UpdateBankScoreText();
        UpdateFieldScoreText();
        UpdateUserLevelText();
        UpdateUserMultiplierText();
    }

    private void UpdateLastScoreText()
    {
        int lastScore = gameManager.LastScore;
        lastScoreText.text = "$" + lastScore;
    }

    private void UpdateHighScoreText()
    {
        int highScore = gameManager.HighScore;
        highScoreText.text = "$" + highScore;
    }

    private void UpdateBankScoreText()
    {
        long bankScore = gameManager.BankValue;
        bankScoreText.text = "$" + bankScore;
    }

    private void UpdateFieldScoreText()
    {
        int fieldScore = gameManager.fieldScore;
        fieldScoreText.text = "$" + gameManager.fieldScore;
    }

    private void UpdateUserLevelText()
    {
        float level = gameManager.LevelMultiplier = gameManager.CalculateLevel();
        userLevel.text = $"{level:F3}"; // Display level with 3 decimal place
        if (level > 0.01)
        {
            userLevel.color = Color.white;
        }
        else
        {
            userLevel.color = Color.grey;
        }

        
    }

    private void UpdateUserMultiplierText()
    {
        float multiplier = gameManager.CurrentMultiplier;
        currentMultiplier.text = $"{multiplier:F2}"; // Display level with 2 decimal place
    }

}