//12
using System;
using System.Collections.Generic;

public class CardManager
{
    public List<Card> cardTypes;
    public List<CardInstance> availableCards;
    public List<CardInstance> ownedCards;
    private int startingCards = 4;
    SliderController sliderController;

    // A dictionary to map card Name/type to card instances.
    private Dictionary<string, Card> cardIdToCardMap;

    public CardManager(List<Card> cardsList)
    {
        cardTypes = new List<Card>(cardsList); // Create a copy of the cardTypes list
        ownedCards = new List<CardInstance>();
        availableCards = new List<CardInstance>();

        //Create a temporary copy of the cardTypes list for pulling out unique cards
        List < Card > tempCardTypes = new List<Card>(cardTypes);

        Card basicCard = GetCardByName("The Basic");
        AddNewOwnedCard(basicCard);
        Card multiplyCard = GetCardByName("Multiply");
        AddNewOwnedCard(multiplyCard);
        Card addManaCard = GetCardByName("Add Mana");
        AddNewOwnedCard(addManaCard);


        // Initialize the ownedCards and availableCards lists with 4 random unique cards
        for (int i = 0; i <= startingCards - ownedCards.Count; i++)
        {
            Card randomCard = GetRandomCard(tempCardTypes);
            if (randomCard != null)
            {
                AddNewOwnedCard(randomCard);
            }
        }

        //AddTestingCards();

        // Initialize the dictionary.
        cardIdToCardMap = new Dictionary<string, Card>();
        foreach (Card card in cardTypes)
        {
            cardIdToCardMap[card.cardName] = card; // Assuming cardName is unique and serves as the card's ID.
        }
    }

    public List<CardInstance> GetOwnedCards()
    {
        return new List<CardInstance>(ownedCards);
    }

    public Card GetCardByName(string name)
    {
        // This assumes that every card in cardTypes has a unique name.
        Card foundCard = cardTypes.Find(card => card.cardName == name);

        return foundCard;
    }

    // Set the list of owned cards, creating new CardInstances from the provided data.
    public void SetOwnedCards(List<CardInstanceData> ownedCardsData)
    {
        ownedCards.Clear();

        foreach (CardInstanceData data in ownedCardsData)
        {

            Card card = GetCardByName(data.cardName);

            CardInstance cardInstance = new CardInstance(card, this, data.level, data.rarity)
            {
                timesPlayed = data.timesPlayed,
                nextUpgradeExtra = data.nextUpgradeExtra,
                UpgradeCost = data.UpgradeCost
            };

            ownedCards.Add(cardInstance);
        }

        ResetAvailableCards();
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
        CardInstance cardInstance = new CardInstance(card, this, 1, 1);
        ownedCards.Add(cardInstance);
        availableCards.Add(cardInstance);
    }

    public void BuyNewOwnedCard(Card card, float min, float max)
    {
        int rarity = GetRandomRarityBetweenBounds((int)min, (int)max);
        CardInstance cardInstance = new CardInstance(card, this, 1, rarity);
        ownedCards.Add(cardInstance);
        availableCards.Add(cardInstance);
    }

    // for checking all card values
    public void AddTestingCards()
    {
        for (int i = 0; i < cardTypes.Count; i++)
        {
            ownedCards.Add(new CardInstance(cardTypes[i], this, 1, 1));
            ownedCards.Add(new CardInstance(cardTypes[i], this, 1, 30));
            ownedCards.Add(new CardInstance(cardTypes[i], this, 1, 100));

            ownedCards.Add(new CardInstance(cardTypes[i], this, 10, 1));
            ownedCards.Add(new CardInstance(cardTypes[i], this, 10, 50));
            ownedCards.Add(new CardInstance(cardTypes[i], this, 10, 100));

            ownedCards.Add(new CardInstance(cardTypes[i], this, 100, 1));
            ownedCards.Add(new CardInstance(cardTypes[i], this, 100, 80));
            ownedCards.Add(new CardInstance(cardTypes[i], this, 100, 100));
        }
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

    private int GetRandomRarityBetweenBounds(int min, int max)
    {
        var randomizer = new System.Random();
        var randomDouble = randomizer.NextDouble();
        double probabilityPower = 3;

        var result = Math.Floor(min + (max + 1 - min) * (Math.Pow(randomDouble, probabilityPower)));
        return (int)result;
    }
}