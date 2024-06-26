﻿//3
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DrawCardButton : MonoBehaviour, IPointerDownHandler {

    public Text drawPriceText;
    public Text deckSizeText;

    public void Update()
    {
        UpdateDeckRemaining(); //todo should maybe handle this elsewhere
    }

    public void OnPointerDown(PointerEventData eventData) {
        GameManager gameManager = GameManager.Instance;

        if (gameManager.cardManager.availableCards.Count > 0)
        {
            if (gameManager.SpendRound(gameManager.DrawCost))
            {
                DrawCard drawCardComponent = FindObjectOfType<DrawCard>();
                drawCardComponent.DrawCards(1);
                gameManager.DrawCost *= 3;
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
        drawPriceText.text = "$" + GameManager.Instance.DrawCost;
    }

    public void UpdateDeckRemaining()
    {
        deckSizeText.text = GameManager.Instance.cardManager.availableCards.Count.ToString() + "/" + GameManager.Instance.cardManager.ownedCards.Count.ToString();
    }
}