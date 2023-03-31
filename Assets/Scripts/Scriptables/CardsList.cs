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
    public float cardStrength = 0.2f;
    public int startingLevel = 1;

    public void Initialize()
    {
        cards.Add(CreateBasicCard());
        cards.Add(CreateDoubleCard());
        cards.Add(CreateDrawCardsCard());
        cards.Add(CreateManaResetCard());
        cards.Add(CreateRandomCoinGainCard());

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

//        float strength = 1 + (cardStrength * basicCard.level * upgradeStrength);
        float strength = basicCard.level;

        basicCard.Actions = new List<Action<GameManager>>
        {
            (gameManager) => {
                gameManager.IncreaseScore((int) strength);
                gameManager.DecreaseMana(10);
                Debug.Log("basic card played");
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
        float strength = 1 + (0.3f * cardStrength * doubleCard.level * upgradeStrength);
        doubleCard.description = strength + "x your score, half your mana";
        doubleCard.Actions = new List<Action<GameManager>>
        {
            (gameManager) => {
                int startScore = gameManager.fieldScore;
                gameManager.IncreaseScore((int) (startScore * strength) - startScore);
                gameManager.DecreaseMana(gameManager.mana * 0.5f);
            }
        };

        return doubleCard;
    }

    private Card CreateDrawCardsCard()
    {
        Card drawCardsCard = Card.CreateInstance(
            "Draw Cards",
            null,
            Resources.Load<Sprite>("CardImages/Cards"),
            null,
            startingLevel,
            baseCardManaCost
        );

        float strength = 1 + (cardStrength * drawCardsCard.level * upgradeStrength);

        drawCardsCard.description = "Draw 1 to " + strength + " new cards";
        drawCardsCard.Actions = new List<Action<GameManager>>
        {
            (gameManager) => {
                Debug.Log("Draw Cards card played");
                DrawCard drawCard = FindObjectOfType<DrawCard>();
                drawCard.DrawCards( (int) UnityEngine.Random.Range(1,strength));
                gameManager.DecreaseMana(10);
            }
        };

        return drawCardsCard;
    }

    private Card CreateManaResetCard()
    {
        Card manaReset = Card.CreateInstance(
            "I NEED MORE TIME",
            null,
            Resources.Load<Sprite>("CardImages/Clock"),
            null,
            startingLevel,
            0
        );

        float strength = 5 - (cardStrength * manaReset.level * upgradeStrength);

        manaReset.description = "Costs $" + strength + "x Uses \n Full Mana";
        manaReset.Actions = new List<Action<GameManager>>
        {
            (gameManager) => {
                if (gameManager.SpendRound((int) strength))
                {
                    gameManager.ResetMana();
                }
                Debug.Log("Mana Reset");
            }
        };

        return manaReset;
    }


    //should maybe multiply, but a loss drops bank to half
    private Card CreateRandomCoinGainCard()
    {
        Card randomCoinGainCard = Card.CreateInstance(
            "Random Coin Gain",
            null,
            Resources.Load<Sprite>("CardImages/RandomCoinGain"),
            null,
            startingLevel,
            baseCardManaCost
        );

        float strength = 1 + (cardStrength * randomCoinGainCard.level * upgradeStrength);

        int minCoinGain = (int) (-0.8f * strength);
        int maxCoinGain = (int) (1.0f * strength);

        randomCoinGainCard.description = "Random coin gain in range" + minCoinGain + "to "+ maxCoinGain;

        randomCoinGainCard.Actions = new List<Action<GameManager>>
        {
            (gameManager) => {
                int coinGain = UnityEngine.Random.Range(minCoinGain, maxCoinGain + 1);
                gameManager.IncreaseScore( (int) coinGain);

                Debug.Log($"Random Coin Gain card played, gained {coinGain} coins");
            }
        };

        return randomCoinGainCard;
    }

}