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
    public float speedToZero = 3f;
    public delegate float FormulaDelegate(CardInstance cardInstance);


    public void Initialize()
    {
        cards = new List<Card>(); // Initialize the cards list before adding elements to it

        cards.Add(CreateBasicCard());
        cards.Add(CreateDoubleCard());
        cards.Add(CreateManaResetCard());
        //cards.Add(CreateDrawCardsCard());
        //cards.Add(CreateRandomCoinGainCard());

    }

    private Card CreateBasicCard()
    {
        float cardCost_multi = 0.4f;
        float cardCost_fixed = 10f;

        Card basicCard = Card.CreateInstance(
            "The Basic",
            "Increase score by 1 x level",
            Resources.Load<Sprite>("CardImages/Coin"),
            null,
            startingLevel,
            baseCardManaCost
        );


        FormulaDelegate costFormula = (cardInstance) => {
            float baseCost = cardCost_fixed + (GameManager.Instance.mana * cardCost_multi);
            float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = 1/(1 + x/100)^speed
            return (float)(baseCost * approachZero);
        };
        basicCard.CostFormula = costFormula;

        FormulaDelegate rewardFormula = (cardInstance) => {
            //return 10f + (float)Math.Pow(1f + (cardInstance.level / 30f), 10); //f(x) = 1 + (1 + x/30)^10
            return 10f + (cardInstance.level * 10f); //f(x) = x * 10, to keep the increase liniar

        };
        basicCard.RewardFormula = rewardFormula;

        basicCard.Actions = new List<Action<GameManager, CardInstance>>
        {
            (gameManager, cardInstance) => {
                float reward = rewardFormula(cardInstance);
                gameManager.IncreaseScore((int) reward);

                float cost = costFormula(cardInstance);
                gameManager.DecreaseMana((int) cost);
            }
        };


        
        basicCard.IsAffordable = (cardInstance, gameManager) => {
            return gameManager.mana >= costFormula(cardInstance);
        };

        return basicCard;
    }

    private Card CreateDoubleCard()
    {

        float cardCost = 40f;

        Card doubleCard = Card.CreateInstance(
            "Multiply",
            "x your score \n half your mana",
            Resources.Load<Sprite>("CardImages/CoinBag"),
            null,
            startingLevel,
            baseCardManaCost
        );

        FormulaDelegate costFormula = (cardInstance) => {
            float baseCost = cardCost;
            float approachZero = (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = 1/(1 + x/100)^speed
            return (float)(baseCost * approachZero);
        };
        doubleCard.CostFormula = costFormula;

        FormulaDelegate rewardFormula = (cardInstance) => {
            GameManager gameManager = FindObjectOfType<GameManager>();
            int startScore = gameManager.fieldScore;
            float approachInf = (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = (1 + x/100)^speed
            return startScore * approachInf;
        };
        doubleCard.RewardFormula = rewardFormula;

        doubleCard.Actions = new List<Action<GameManager, CardInstance>>
        {
            (gameManager, cardInstance) => {
                float reward = rewardFormula(cardInstance); 
                gameManager.IncreaseScore((int) reward);

                float cost = costFormula(cardInstance);
                gameManager.DecreaseMana((int) cost);
            }
        };



        doubleCard.IsAffordable = (cardInstance, gameManager) => {
            return gameManager.mana >= costFormula(cardInstance);
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

        FormulaDelegate costFormula = (cardInstance) => {
            float baseCost = cardCost;
            float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = 1/(1 + x/100)^speed
            return (float)(baseCost * approachZero);
        };
        manaReset.CostFormula = costFormula;

        FormulaDelegate rewardFormula = (cardInstance) => {
            float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = 1/(1 + x/100)^speed
            return 100f - (80f * approachZero); //20 > 22 > ... > 100

        };
        manaReset.RewardFormula = rewardFormula;

        manaReset.Actions = new List<Action<GameManager, CardInstance>>
        {
            (gameManager, cardInstance) => {
                float reward = rewardFormula(cardInstance);
                gameManager.IncreaseMana((int) reward);

                float cost = costFormula(cardInstance);
                gameManager.DecreaseScore((int) cost);
            }
        };

        manaReset.IsAffordable = (cardInstance, gameManager) => {
            return gameManager.fieldScore >= costFormula(cardInstance);
        };

        return manaReset;
    }
}