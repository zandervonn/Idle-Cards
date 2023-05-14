//12
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager
{
    public List<Card> cardTypes;
    public List<CardInstance> availableCards;
    public List<CardInstance> ownedCards;
    private int startingCards = 4;

    public CardManager(List<Card> cardsList)
    {
        cardTypes = new List<Card>(cardsList); // Create a copy of the cardTypes list
        ownedCards = new List<CardInstance>();
        availableCards = new List<CardInstance>();

        // Initialize the ownedCards and availableCards lists with 4 random unique cards
        for (int i = 0; i < startingCards; i++)
        {
            Card randomCard = GetRandomCard(cardTypes);
            if (randomCard != null)
            {
                AddNewOwnedCard(randomCard);
            }
        }
    }

    private Card GetRandomCard(List<Card> cardsList)
    {
        if (cardsList.Count == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, cardsList.Count);
        Card randomCard = cardsList[randomIndex];
        cardsList.RemoveAt(randomIndex); // Remove the selected card from the list
        return randomCard;
    }

    public void AddNewOwnedCard(Card card)
    {
        int rarity = GetRandomRarity();
        CardInstance cardInstance = new CardInstance(card, this, 1, rarity);
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

    private int GetRandomRarity()
    {
        var randomizer = new System.Random();
        var randomDouble = randomizer.NextDouble();
        int min = 1;
        int max = 100;
        double probabilityPower = 3;

        var result = Math.Floor(min + (max + 1 - min) * (Math.Pow(randomDouble, probabilityPower)));
        return (int)result;
    }
}