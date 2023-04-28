//3
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResetRound : MonoBehaviour, IPointerDownHandler {

    private float resetCost = 1;
    public Text resetCostText;
    public DrawCardButton drawCardButton;

    public void OnPointerDown(PointerEventData eventData) {
        GameManager gameManager = GameManager.Instance;
        if (gameManager.SpendBank((int)resetCost))
        {
            gameManager.OnResetRound();
            UpdateResetPriceText();
        }
    }

    public void UpdateResetPriceText()
    {
        GameManager gameManager = GameManager.Instance;
        resetCost = (gameManager.TotalMoneyEarned * 0.2f) + 1f;
        resetCostText.text = "$" +  (resetCost).ToString("F0");
    }
}