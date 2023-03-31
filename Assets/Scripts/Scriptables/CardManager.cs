//12
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager
{
    public List<Card> cardTypes;
    public List<Card> availableCards;
    public List<Card> ownedCards;
    public List<Card> selectedCards; // For storing cards that are selected to be drawable

    public CardManager(List<Card> cardsList)
    {
        cardTypes = cardsList;
        ownedCards = new List<Card>();
        availableCards = new List<Card>();
        selectedCards = new List<Card>();

        // Initialize the ownedCards and availableCards lists
        foreach (Card card in cardTypes)
        {
            ownedCards.Add(card);
            availableCards.Add(card);
//            ownedCards.Add(Card.CreateInstance(card));
//            availableCards.Add(Card.CreateInstance(card));
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

    public void SelectCard(Card card)
    {
        if (!selectedCards.Contains(card))
        {
            selectedCards.Add(card);
        }
    }

    public void DeselectCard(Card card)
    {
        if (selectedCards.Contains(card))
        {
            selectedCards.Remove(card);
        }
    }
}