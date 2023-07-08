//3
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResetRound : MonoBehaviour, IPointerDownHandler {

    public Text resetCostText;
    public DrawCardButton drawCardButton;
    private float resetCostDecreaseSpeed = 0.9985f; //95>9985

    public void Start()
    {
        UpdateResetPriceText();
    }

    float updateInterval = 0.3f;
    float updateTimer = 0f;

    public void Update()
    {
        updateTimer += Time.deltaTime;
        if (updateTimer >= updateInterval)
        {
            UpdateResetPriceText();
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        GameManager gameManager = GameManager.Instance;
        if (gameManager.SpendBank((int)gameManager.ResetCost))
        {
            gameManager.OnResetRound();
            IncreaseResetPriceText();
        }
    }

    public void IncreaseResetPriceText()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.ResetCost += (gameManager.TotalMoneyEarned * 0.025f) + 1f;
        resetCostText.text = "$" +  (gameManager.ResetCost).ToString("F0");
    }

    public void UpdateResetPriceText()
    {
        GameManager gameManager = GameManager.Instance;
        if(gameManager.ResetCost > 1.0f)
        {
            gameManager.ResetCost *= resetCostDecreaseSpeed; 
        }
        resetCostText.text = "$" + (gameManager.ResetCost).ToString("F0");
    }
}