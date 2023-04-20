﻿//13
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RemoveCardButton : MonoBehaviour, IPointerDownHandler
{
    private GameManager gameManager;
    private CardDisplay cardDisplay;
    public Text removeCardPrice;

    private void Start()
    {
        gameManager = GameManager.Instance;
        cardDisplay = GetComponentInParent<CardDisplay>();
        UpdateRemoveCardPrice(); 
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        GameManager gameManager = GameManager.Instance;
        int removeCost = gameManager.RemoveCost;
        Debug.Log("remove card for $" + removeCost);
        if (gameManager.SpendBank(gameManager.RemoveCost))
        {
            cardDisplay.cardInstance.Remove();

            // Update the remove card cost
            gameManager.UpdateRemoveCost();

            // Update remove card prices for all card instances
            UpdateAllRemoveCardPrices();

            // Update the deck display
            DeckManager deckManager = FindObjectOfType<DeckManager>();
            if (deckManager != null && deckManager.isDeckVisible)
            {
                deckManager.DisplayCards();
            }

        }
    }
    private void UpdateAllRemoveCardPrices()
    {
        RemoveCardButton[] removeCardButtons = FindObjectsOfType<RemoveCardButton>();
        foreach (RemoveCardButton button in removeCardButtons)
        {
            button.UpdateRemoveCardPrice();
        }
    }

    private void UpdateRemoveCardPrice()
    {
        int removeCost = gameManager.RemoveCost;
        Debug.Log("remove card price set as" + removeCost);

        removeCardPrice.text = "$" + removeCost;
    }
}