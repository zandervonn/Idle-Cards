//6
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardsList : MonoBehaviour
{
    public List<Card> cards;
    public CardDisplay cardPrefab;
    public Transform cardParent;
    public int baseCardManaCost = 10;
    public float upgradeStrength = 1.5f;
    public float cardStrength = 1f;
    public int startingLevel = 1;

    public void Initialize()
    {
        cards = new List<Card>(); // Initialize the cards list before adding elements to it

        cards.Add(CreateBasicCard());
        cards.Add(CreateDoubleCard());
        //cards.Add(CreateDrawCardsCard());
        //cards.Add(CreateManaResetCard());
        //cards.Add(CreateRandomCoinGainCard());

        Debug.Log("CardsList Initialized. Number of cards: " + cards.Count);
    }

    private Card CreateBasicCard()
    {
        Card basicCard = Card.CreateInstance(
            "The Basic",
            "Increase score by 1 x level",
            Resources.Load<Sprite>("CardImages/Coin"),
            null,
            startingLevel,
            baseCardManaCost
        );

        basicCard.Actions = new List<Action<GameManager, CardInstance>>
        {
            (gameManager, cardInstance) => {
                gameManager.IncreaseScore((int) cardInstance.level); // Use cardInstance.level instead of basicCard.level
                gameManager.DecreaseMana(10);
                Debug.Log("basic card played level: " + cardInstance.level);
            }
        };

        return basicCard;
    }

    private Card CreateDoubleCard()
    {

        Card doubleCard = Card.CreateInstance(
            "Multiply",
            "x your score \n half your mana",
            Resources.Load<Sprite>("CardImages/CoinBag"),
            null,
            startingLevel,
            baseCardManaCost
        );

        doubleCard.description = "x your score, half your mana";
        doubleCard.Actions = new List<Action<GameManager, CardInstance>>
        {
            (gameManager, cardInstance) => {
                int startScore = gameManager.fieldScore;
                gameManager.IncreaseScore((int) (startScore * cardInstance.level) - startScore); // Use cardInstance.level instead of doubleCard.level
                gameManager.DecreaseMana(gameManager.mana * 0.5f);
                Debug.Log("double card played level: " + cardInstance.level);
            }
        };

        return doubleCard;
    }

}