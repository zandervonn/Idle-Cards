//11
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class BuyNewCard : MonoBehaviour, IPointerDownHandler
{

    public CardsList cardsList;
    public int cardCost = 1;
    public Button buyButton;
    public GameManager gameManager;

    private void Start()
    {
        cardsList = FindObjectOfType<CardsList>();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BuyCard();
    }

    public void BuyCard()
    {
        gameManager = FindObjectOfType<GameManager>();
        cardsList = FindObjectOfType<CardsList>();
        if (gameManager.SpendBank(cardCost))
        {
            // Get a random card from the list of card types (cardsList.cards)
            int cardIndex = Random.Range(0, cardsList.cards.Count);
            Card card = cardsList.cards[cardIndex];

            // Add the random card to the ownedCards list
            gameManager.cardManager.AddNewOwnedCard(card);
        }
        else
        {
            Debug.LogWarning("Not enough bank value to buy a card.");
        }
    }
}
