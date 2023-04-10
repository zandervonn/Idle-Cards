//3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DrawCardButton : MonoBehaviour, IPointerDownHandler
{

    public GameManager gameManager; // Make it public
    public Text drawPriceText;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager gameManager = GameManager.Instance;

        // Remove the local variable declaration
        if (gameManager.cardManager.availableCards.Count > 0)
        {
            if (gameManager.SpendRound(1 + gameManager.BuyCost))
            {
                DrawCard drawCardComponent = FindObjectOfType<DrawCard>();
                drawCardComponent.DrawCards(1);
                Debug.Log("buy cost before:" + gameManager.BuyCost);
                gameManager.BuyCost *= 2;
                Debug.Log("buy cost updated:" + gameManager.BuyCost);
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
        GameManager gameManager = GameManager.Instance;
        // Remove the local variable declaration
        Debug.Log("buy cost:" + gameManager.BuyCost);
        drawPriceText.text = "$" + gameManager.BuyCost;
    }
}