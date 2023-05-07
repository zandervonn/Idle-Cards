//6
using System.Collections.Generic;
using UnityEngine;
using System;
using static CardsList.ToTheBankCard;

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
        cards.Add(new ToTheBankCard());
        cards.Add(new MortgageCard());
        cards.Add(new PanicModeCard());
        cards.Add(new LottoCard());
        cards.Add(new ResetDrawCostCard());
    }

    public class BasicCard : Card
    {
        public BasicCard()
        {
            cardName = "The Basic";
            description = "Increase score by fixed \n costs % of mana";
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
            description = "Multiply your score \n Fixed mana cost";
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
            description = "Increase mana by % \n Costs fixed coins";
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
            description = "Draw fixed cards \n Costs fixed mana";
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
            description = "Increase score exponential to mana \n Costs % of mana ";
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

    public class ToTheBankCard : Card
    {
        public ToTheBankCard()
        {
            cardName = "To the Bank";
            description = "Add % of current score to bank \n Costs fixed mana ";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 10;
            rarity = 1;

            CardCost = new ManaCardCost(
                (cardInstance) =>
                {
                    float baseCost = 30f;
                    float approachZero = (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    return (float)(baseCost * approachZero);
                }
            );

            CardReward = new BankCardReward(
                (cardInstance) =>
                {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    int startScore = gameManager.fieldScore;
                    float approachInf = (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    float rarityAdd = 1 + (cardInstance.rarity) / 25;
                    return 1 + ((int)Math.Pow((startScore * approachInf * rarityAdd) / 10, 1.3f));
                }
            );
        }

        public class MortgageCard : Card
        {
            public MortgageCard()
            {
                cardName = "Mortgage";
                description = "Add % of bank to hand \n Costs % of mana";
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
                    (cardInstance) =>
                    {
                        GameManager gameManager = FindObjectOfType<GameManager>();
                        int startScore = gameManager.BankValue;
                        float approachInf = (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                        float rarityAdd = 1 + (cardInstance.rarity) / 25;
                        return (int)(startScore * approachInf * rarityAdd) / 10;
                    }
                );
            }
        }

        public class PanicModeCard : Card
        {
            public PanicModeCard()
            {
                cardName = "Panic Mode";
                description = "The closer to 0 mana, the more score \n Costs % of mana";
                artwork = null;
                startingLevel = 1;
                baseCardManaCost = 10;
                rarity = 1;

                CardCost = new ManaCardCost(
                    (cardInstance) => {
                        float baseCost = GameManager.Instance.mana * 0.5f;
                        float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                        return (float)(baseCost * approachZero);
                    }
                );

                CardReward = new ScoreCardReward(
                    (cardInstance) =>
                    {
                        GameManager gameManager = FindObjectOfType<GameManager>();
                        float mana = gameManager.mana;
                        float approachInf = (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                        float rarityAdd = 1 + (cardInstance.rarity) / 25;
                        return (int)Math.Pow((10 / (mana + 1)), 2) * approachInf * rarityAdd;
                    }
                );
            }
        }

        public class LottoCard : Card
        {
            public LottoCard()
            {
                cardName = "The gambler";
                description = "High chance to increase coins by % \n Low chance to lose all coins";
                artwork = null;
                startingLevel = 1;
                baseCardManaCost = 10;
                rarity = 1;

                CardCost = new ScoreCardCost(
                    (cardInstance) => {
                        GameManager gameManager = FindObjectOfType<GameManager>();
                        int startScore = gameManager.fieldScore;
                        int rand = UnityEngine.Random.Range(0, 100);
                        float approachInf = (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                        float rarityAdd = 1 + (cardInstance.rarity) / 25;
                        float odds = 30 / (approachInf * rarityAdd);
                        Debug.Log("odds: " + odds);
                        if (rand < odds) {
                            return startScore;
                        }
                        return 0;
                    }
                );

                CardReward = new ScoreCardReward(
                    (cardInstance) => {
                        GameManager gameManager = FindObjectOfType<GameManager>();
                        int startScore = gameManager.fieldScore;
                        float approachInf = (float)Math.Pow(1f + cardInstance.level / 60f, 3);
                        float rarityAdd = 1 + (cardInstance.rarity) / 10;
                        return startScore * approachInf * rarityAdd;
                    }
                );
            }
        }

        public class ResetDrawCostCard : Card
        {
            public ResetDrawCostCard()
            {
                cardName = "Reset Draw Cost";
                description = "Draw card cost = 0 \n Costs % of mana";
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

                CardReward = new DrawCostCardReward(
                    (cardInstance) => {
                        return 0;
                    }
                );
            }
        }
    }

    /**
o	        add copy of next card played
o	    one time use cards, high value 
o	        draw non-owned card
o	//add value directly to the bank
o	    x coins and x mana, costs card
o	x coins x cards, costs mana
o	x mana and x cards, costs coins
o	//x% of your bank value
o	        draw 2 discard 1
o	if mana is below x, gain x
    the less mana the more value
o	if mana is above x, gain x
o	    freeze mana decrease with time rate
o	        gain x coins, burn 2 random cards
o	        re shuffle discard into deck
o	//reset card draw cost
o	//lotto, clears bank or gives coins
o	    next cost draws from bank * 10
o	    convert mana to coins, deplete all mana

**/

}



