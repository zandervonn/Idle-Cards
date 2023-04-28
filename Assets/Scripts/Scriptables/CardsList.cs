//6
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
        cards = new List<Card>();

        cards.Add(new BasicCard());
        cards.Add(new DoubleCard());
        cards.Add(new ManaResetCard());
        cards.Add(new DrawCardsCard());
        cards.Add(new ManaLeftCard());
    }

    public class BasicCard : Card
    {
        public BasicCard()
        {
            cardName = "The Basic";
            description = "Increase score by 1 x level";
            artwork = Resources.Load<Sprite>("CardImages/Coin");
            startingLevel = 1;
            baseCardManaCost = 10;
            rarity = 50;

            CardCost = new ManaCardCost(
                (cardInstance) => {
                    float baseCost = 10f + (GameManager.Instance.mana * 0.4f);
                    float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    return (float)(baseCost * approachZero);
                }
            );

            CardReward = new ScoreCardReward(
                (cardInstance) => {
                    float rarityAdd = cardInstance.rarity / 10;
                    return 10f + (cardInstance.level * (5f + rarityAdd));
                }
            );
        }
    }

    public class DoubleCard : Card
    {
        public DoubleCard()
        {
            cardName = "Multiply";
            description = "x your score \n half your mana";
            artwork = Resources.Load<Sprite>("CardImages/CoinBag");
            startingLevel = 1;
            baseCardManaCost = 40;
            rarity = 1;

            CardCost = new ManaCardCost(
                (cardInstance) => {
                    float baseCost = 40f;
                    float approachZero = (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    return (float)(baseCost * approachZero);
                }
            );

            CardReward = new ScoreCardReward(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    int startScore = gameManager.fieldScore;
                    float approachInf = (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    float rarityAdd = 1 + (cardInstance.rarity) / 25;
                    return startScore * approachInf * rarityAdd;
                }
            );
        }
    }

    public class ManaResetCard : Card
    {
        public ManaResetCard()
        {
            cardName = "I NEED MORE TIME";
            description = null;
            artwork = Resources.Load<Sprite>("CardImages/Clock");
            startingLevel = 1;
            baseCardManaCost = 0;
            rarity = 1;

            CardCost = new ScoreCardCost(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    int startScore = gameManager.fieldScore;
                    float baseCost = 30f;
                    float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    return (float)(startScore * ((1f / 100f) * baseCost * approachZero));
                }
            );

            CardReward = new ManaCardReward(
                (cardInstance) => {
                    float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    float rarityAdd = 1 + (cardInstance.rarity / 50);
                    return (100f - (80f * approachZero)) * rarityAdd;
                }
            );
        }
    }

    public class DrawCardsCard : Card
    {
        public DrawCardsCard()
        {
            cardName = "Draw Cards";
            description = null;
            artwork = Resources.Load<Sprite>("CardImages/Cards");
            startingLevel = 1;
            baseCardManaCost = 30;
            rarity = 1;

            CardCost = new ManaCardCost(
                (cardInstance) => {
                    float baseCost = 30f;
                    float approachZero = (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    return (float)(baseCost * approachZero);
                }
            );

            CardReward = new CardCardReward(
                (cardInstance) => {
                    float rarityAdd = 2 + (cardInstance.rarity / 25);
                    return rarityAdd;
                }
            );
        }
    }

    public class ManaLeftCard : Card
    {
        public ManaLeftCard()
        {
            cardName = "Mana Left";
            description = "Multiply current mana, add it to score";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 10;
            rarity = 1;

            CardCost = new ManaCardCost(
                (cardInstance) => {
                    float baseCost = GameManager.Instance.mana * 0.3f;
                    float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    return (float)(baseCost * approachZero);
                }
            );

            CardReward = new ScoreCardReward(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    float mana = gameManager.mana;
                    float approachInf = (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    float rarityAdd = 1 + (cardInstance.rarity) / 25;
                    return (mana * approachInf * rarityAdd) / 10;
                }
            );
        }
    }

}



