//15
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class BuyNewCard : MonoBehaviour, IPointerDownHandler
{

    private CardsList cardsList;
    
    public Text cardCostText;
    private GameManager gameManager;
    //private int cardCostMultiplier = 5; // testing
    private int cardCostMultiplier = 1;
    //private int cardCost = 10; //testing
    private int cardCost = 1;

    private void Start()
    {
        cardsList = FindObjectOfType<CardsList>();
        gameManager = FindObjectOfType<GameManager>();
        cardCostText.text = "$" + cardCost;
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
            cardCost *= cardCostMultiplier;
            cardCostText.text = "$" + cardCost;

            // Update the deck display
            DeckManager deckManager = FindObjectOfType<DeckManager>();
            if (deckManager != null && deckManager.isDeckVisible)
            {
                deckManager.DisplayCards();
            }
        }
        else
        {
            Debug.LogWarning("Not enough bank value to buy a card.");
        }
    }
}
