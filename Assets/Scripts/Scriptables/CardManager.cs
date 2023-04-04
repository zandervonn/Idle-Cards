//12
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager
{
    public List<Card> cardTypes;
    public List<Card> availableCards;
    public List<Card> ownedCards;

    void awake()
    {
        foreach (Card card in cardTypes)
        {
            Card newCard = new Card(card);
            ownedCards.Add(newCard);
            availableCards.Add(newCard);
        }
    }

    public CardManager(List<Card> cardsList)
    {
        cardTypes = cardsList;
        ownedCards = new List<Card>();
        availableCards = new List<Card>();

        // Initialize the ownedCards and availableCards lists
        foreach (Card card in cardTypes)
        {
            ownedCards.Add(card);
            availableCards.Add(card);
        }
    }

    public void AddNewOwnedCard(Card card)
    {
        ownedCards.Add(card);
        availableCards.Add(card);
    }

    public void ResetAvailableCards()
    {
        availableCards = new List<Card>(ownedCards);
    }
}