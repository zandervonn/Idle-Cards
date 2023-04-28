//3
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DrawCardButton : MonoBehaviour, IPointerDownHandler {

    public Text drawPriceText;

    public void OnPointerDown(PointerEventData eventData) {
        GameManager gameManager = GameManager.Instance;

        // Remove the local variable declaration
        if (gameManager.cardManager.availableCards.Count > 0)
        {
            if (gameManager.SpendRound(1 + gameManager.BuyCost))
            {
                DrawCard drawCardComponent = FindObjectOfType<DrawCard>();
                drawCardComponent.DrawCards(1);
                gameManager.BuyCost *= 4;
                UpdateDrawPriceText();
            }
        }
        else
        {
            Debug.Log("No cards left");
        }
    }

    public void UpdateDrawPriceText()
    {
        drawPriceText.text = "$" + GameManager.Instance.BuyCost;
    }
}