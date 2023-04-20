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
        cards.Add(CreateManaResetCard());
        //cards.Add(CreateDrawCardsCard());
        //cards.Add(CreateRandomCoinGainCard());

        Debug.Log("CardsList Initialized. Number of cards: " + cards.Count);
    }

    private Card CreateBasicCard()
    {
        float cardCost_multi = 0.4f;
        float cardCost_fixed = 10f;
        float upgradeStrength = 1f;

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
            gameManager.IncreaseScore((int) (10000 + (upgradeStrength  * (cardInstance.level - 1))));
            gameManager.DecreaseMana((int)(cardCost_fixed + (GameManager.Instance.mana * cardCost_multi)));
        }
    };

        basicCard.CostFormula = (cardInstance) => {
            float baseCost = cardCost_fixed + (GameManager.Instance.mana * cardCost_multi);
            float speedToZero = 3; 
            float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = 1/(1 + x/100)^speed
            return (float)(baseCost * approachZero);
        };

        basicCard.IsAffordable = (cardInstance, gameManager) => {
            return gameManager.mana >= basicCard.CostFormula(cardInstance);
        };

        return basicCard;
    }

    private Card CreateDoubleCard()
    {

        float cardCost = 40f;
        float upgradeStrength = 0.1f;

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
                gameManager.IncreaseScore((int) (startScore * upgradeStrength * ( cardInstance.level + 1 )));
                gameManager.DecreaseMana((int) cardCost);
                Debug.Log("double card played level: " + cardInstance.level);
            }
        };

        doubleCard.CostFormula = (cardInstance) => {
            float baseCost = cardCost;
            float speedToZero = 3;
            float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = 1/(1 + x/100)^speed
            return (float)(baseCost * approachZero);
        };

        doubleCard.IsAffordable = (cardInstance, gameManager) => {
            return gameManager.mana >= doubleCard.CostFormula(cardInstance);
        };

        return doubleCard;
    }

    private Card CreateManaResetCard()
    {

        float cardCost = 10f;
        float upgradeStrength = 0.5f;

        Card manaReset = Card.CreateInstance(
            "I NEED MORE TIME",
            null,
            Resources.Load<Sprite>("CardImages/Clock"),
            null,
            startingLevel,
            0
        );

        float strength = 25 + (manaReset.level * upgradeStrength);

        manaReset.description = "Costs $" + strength + "x Uses \n Full Mana";
        manaReset.Actions = new List<Action<GameManager, CardInstance>>
        {
            (gameManager, cardInstance) => {
                if (gameManager.SpendRound((int) cardCost))
                {
                    gameManager.IncreaseMana(strength); 
                }
                Debug.Log("Mana Reset");
            }
        };

        manaReset.CostFormula = (cardInstance) => {
            float baseCost = cardCost;
            float speedToZero = 3;
            float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = 1/(1 + x/100)^speed
            return (float) (baseCost * approachZero);
        };

        manaReset.IsAffordable = (cardInstance, gameManager) => {
            return gameManager.fieldScore >= manaReset.CostFormula(cardInstance);
        };

        return manaReset;
    }


}