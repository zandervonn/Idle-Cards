//12
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager
{
    public List<Card> cardTypes;
    public List<CardInstance> availableCards;
    public List<CardInstance> ownedCards;

    public CardManager(List<Card> cardsList)
    {
        cardTypes = cardsList;
        ownedCards = new List<CardInstance>();
        availableCards = new List<CardInstance>();

        // Initialize the ownedCards and availableCards lists
        foreach (Card card in cardTypes)
        {
            AddNewOwnedCard(card);
        }
    }
    public void AddNewOwnedCard(Card card)
    {
        CardInstance cardInstance = new CardInstance(card, this);
        ownedCards.Add(cardInstance);
        availableCards.Add(cardInstance);
    }

    public void ResetAvailableCards()
    {
        availableCards = new List<CardInstance>(ownedCards);
    }

    public void RemoveCard(CardInstance cardToRemove)
    {
        ownedCards.Remove(cardToRemove);
    }
}