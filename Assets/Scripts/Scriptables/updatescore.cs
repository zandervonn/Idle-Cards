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
    }

    private void Update()
    {
        UpdateLastScoreText();
        UpdateHighScoreText();
        UpdateBankScoreText();
        UpdateFieldScoreText();
        UpdateUserLevelText();
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
        int bankScore = gameManager.BankValue;
        bankScoreText.text = "$" + bankScore;
    }

    private void UpdateFieldScoreText()
    {
        int fieldScore = gameManager.fieldScore;
        fieldScoreText.text = "$" + gameManager.fieldScore;
    }

    private void UpdateUserLevelText()
    {
        float level = GameManager.Instance.CalculateLevel();
        userLevel.text = $"{level:F1}"; // Display level with 1 decimal place
    }

}