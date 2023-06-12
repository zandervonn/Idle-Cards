//3
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResetRound : MonoBehaviour, IPointerDownHandler {

    public Text resetCostText;
    public DrawCardButton drawCardButton;

    public void Start()
    {
        UpdateResetPriceText();
    }

    public void OnPointerDown(PointerEventData eventData) {
        GameManager gameManager = GameManager.Instance;
        if (gameManager.SpendBank((int)gameManager.ResetCost))
        {
            gameManager.OnResetRound();
            UpdateResetPriceText();
        }
    }

    public void UpdateResetPriceText()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.ResetCost = (int) ((gameManager.TotalMoneyEarned * 0.05f) + 1f);
        resetCostText.text = "$" +  (gameManager.ResetCost).ToString("F0");
    }
}