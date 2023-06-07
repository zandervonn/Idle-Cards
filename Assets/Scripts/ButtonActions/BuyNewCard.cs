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

    private void Start()
    {
        cardsList = FindObjectOfType<CardsList>();
        gameManager = GameManager.Instance;
        cardCostText.text = "$" + gameManager.BuyCost;
    }


    private void Update()
    {
        gameManager = GameManager.Instance;
        cardCostText.text = "$" + gameManager.BuyCost;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BuyCard();
    }

    public void BuyCard()
    {
        cardsList = FindObjectOfType<CardsList>();

        if (gameManager.SpendBank(gameManager.BuyCost))
            {
            // Get a random card from the list of card types (cardsList.cards)
            int cardIndex = GetRandomCardIndexWithWeight();
            Card card = cardsList.cards[cardIndex];

            // Add the random card to the ownedCards list
            gameManager.cardManager.AddNewOwnedCard(card);
            gameManager.UpdateBuyCost();

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

    int GetRandomCardIndexWithWeight()
    {
        // Calculate total weight
        float totalWeight = 0;
        foreach (Card card in cardsList.cards)
        {
            totalWeight += 1.0f / card.stars;
        }

        // Generate a random number in the range [0, totalWeight)
        float randomWeight = Random.Range(0, totalWeight);

        // Determine which card this random weight corresponds to
        for (int cardIndex = 0; cardIndex < cardsList.cards.Count; ++cardIndex)
        {
            randomWeight -= 1.0f / cardsList.cards[cardIndex].stars;
            if (randomWeight <= 0)
            {
                return cardIndex;
            }
        }

        // This point should never be reached, but return the last card index as a fallback
        return cardsList.cards.Count - 1;
    }


}
