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
        // card cost =                cost * level        = liniar
        // upgrade cost = need to use...  curent cost * cost  = exponetial 
        float cardCost = 0.2f;
        //float upgradeCost = 0.5f;
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
                gameManager.IncreaseScore((int) (10 + (upgradeStrength  * (cardInstance.level - 1))));
                gameManager.DecreaseMana(gameManager.mana * cardCost);
                //gameManager.DecreaseMana((int) cardCost);
            }
        };

        return basicCard;
    }

    private Card CreateDoubleCard()
    {

        float cardCost = 40f;
        //float upgradeCost = 0.8f;
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
                gameManager.IncreaseScore((int) (startScore * upgradeStrength * cardInstance.level));
                //.DecreaseMana(gameManager.mana * cardCost);
                gameManager.DecreaseMana((int) cardCost);
                Debug.Log("double card played level: " + cardInstance.level);
            }
        };

        return doubleCard;
    }

    private Card CreateManaResetCard()
    {

        float cardCost = 10f;
        //float upgradeCost = 0.8f;
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

        return manaReset;
    }

    //private Card CreateDrawCardsCard()
    //{
    //    float cardCost = 10f;
    //    float upgradeCost = 0.8f;
    //    float upgradeStrength = 0.1f;

    //    Card drawCardsCard = Card.CreateInstance(
    //        "Draw Cards",
    //        null,
    //        Resources.Load<Sprite>("CardImages/Cards"),
    //        null,
    //        startingLevel,
    //        baseCardManaCost
    //    );

    //    float strength = 1 + (upgradeStrength * drawCardsCard.level);

    //    drawCardsCard.description = "Draw 1 to " + strength + " new cards";
    //    drawCardsCard.Actions = new List<Action<GameManager, CardInstance>>
    //    {
    //        (gameManager, cardInstance) => {
    //            DrawCard drawCard = FindObjectOfType<DrawCard>();
    //            drawCard.DrawCards( (int)
    //                Math.Floor(Math.Pow((UnityEngine.Random.Range(1,100)/100), Math.Log(strength) / (strength - 1)) * strength) + 1);
    //            gameManager.DecreaseMana(cardCost);
    //        }
    //    };

    //    return drawCardsCard;
    //}


    ////should maybe multiply, but a loss drops bank to half
    //private Card CreateRandomCoinGainCard()
    //{

    //    float cardCost = 20f;
    //    float upgradeCost = 0.8f;
    //    float upgradeStrength = 1f;

    //    Card randomCoinGainCard = Card.CreateInstance(
    //        "Random Coin Gain",
    //        null,
    //        Resources.Load<Sprite>("CardImages/RandomCoinGain"),
    //        null,
    //        startingLevel,
    //        baseCardManaCost
    //    );

    //    float strength = 10 + (randomCoinGainCard.level * upgradeStrength);

    //    int minCoinGain = (int)(-0.8f * strength);
    //    int maxCoinGain = (int)(1.0f * strength);

    //    randomCoinGainCard.description = "Random coin gain in range" + minCoinGain + "to " + maxCoinGain;

    //    randomCoinGainCard.Actions = new List<Action<GameManager, CardInstance>>
    //    {
    //        (gameManager, cardInstance) => {
    //            int coinGain = UnityEngine.Random.Range(minCoinGain, maxCoinGain);
    //            gameManager.IncreaseScore( (int) coinGain);
    //            gameManager.DecreaseMana(cardCost);
    //            Debug.Log($"Random Coin Gain card played, gained {coinGain} coins");
    //        }
    //    };

    //    return randomCoinGainCard;
    //}

}