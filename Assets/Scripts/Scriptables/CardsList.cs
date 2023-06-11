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

        cards.Add(ScriptableObject.CreateInstance<BasicCard>());
        cards.Add(ScriptableObject.CreateInstance<DoubleCard>());
        cards.Add(ScriptableObject.CreateInstance<ManaResetCard>());
        cards.Add(ScriptableObject.CreateInstance<DrawCardsCard>());
        cards.Add(ScriptableObject.CreateInstance<ManaLeftCard>());
        cards.Add(ScriptableObject.CreateInstance<PanicModeCard>());
        cards.Add(ScriptableObject.CreateInstance<LottoCard>());
        cards.Add(ScriptableObject.CreateInstance<DuplicateCard>());

        //cards.Add(ScriptableObject.CreateInstance<MortgageCard>());
        //cards.Add(ScriptableObject.CreateInstance<ManaPauseCard>());
        ////cards.Add(ScriptableObject.CreateInstance<ToTheBankCard>());
        ////cards.Add(ScriptableObject.CreateInstance<ResetDrawCostCard>());

    }

    public class BasicCard : Card
    {
        public BasicCard()
        {
            cardName = "The Basic";
            description = "Increase score by fixed \ncosts % of mana";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 10;
            rarity = 1;
            stars = 1;

            CardCost = new ManaCardCost(
                (cardInstance) => {
                    float baseCost = 10f + (GameManager.Instance.mana * 0.4f);
                    float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    return (float)(baseCost * approachZero);
                }
            );

            CardReward = new ScoreCardReward(
                (cardInstance) => {
                    float rarityAdd = cardInstance.rarity / 20;
                    return 4f + (cardInstance.level * (5f + rarityAdd));
                }
            );
        }
    }

    public class DoubleCard : Card
    {
        public DoubleCard()
        {
            cardName = "Multiply";
            description = "Multiply your score \nFixed mana cost";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 40;
            rarity = 1;
            stars = 2;

            CardCost = new ManaCardCost(
                (cardInstance) => {
                    float baseCost = 40f;
                    float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    return (float)(baseCost * approachZero);
                }
            );

            CardReward = new ScoreCardReward(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    int startScore = gameManager.fieldScore;
                    float approachInf = (float)Math.Pow(1f + cardInstance.level / 100f, 3) -1;
                    float rarityAdd = cardInstance.rarity / 100;
                    return startScore * approachInf * rarityAdd;
                }
            );
        }
    }

    public class ManaResetCard : Card
    {
        public ManaResetCard()
        {
            cardName = "Add Mana";
            description = "Increase mana by % \nCosts fixed coins";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 0;
            rarity = 1;
            stars = 2;

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
            description = "Draw fixed cards \nCosts fixed mana";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 30;
            rarity = 1;
            stars = 3;

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
            description = "Increase score exponential to mana \nCosts % of mana ";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 10;
            rarity = 1;
            stars = 2;

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
            description = "Add % of current score to bank \nCosts fixed mana ";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 10;
            rarity = 1;
            stars = 3;

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

    }

    public class MortgageCard : Card
    {
        public MortgageCard()
        {
            cardName = "Mortgage";
            description = "Add % of bank to hand \nCosts % of mana";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 10;
            rarity = 1;
            stars = 4;

            CardCost = new BankCardCost(
                (cardInstance) => {
                    float baseCost = GameManager.Instance.BankValue * 0.3f;
                    float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    return (float)(baseCost * approachZero);
                }
            );

            CardReward = new ScoreCardReward(
                (cardInstance) =>
                {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    long startScore = gameManager.BankValue;
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
            description = "The closer to 0 mana, the more score \nCosts % of mana";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 10;
            rarity = 1;
            stars = 3;

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
                    return (int)(Math.Pow((10 / (mana + 1)), 2) * approachInf * rarityAdd) + 1;
                }
            );
        }
    }

    public class LottoCard : Card
    {
        public LottoCard()
        {
            cardName = "The gambler";
            description = "High chance to increase coins by % \nLow chance to lose all coins";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 10;
            rarity = 1;
            stars = 3;

            CardCost = new ScoreCardCost(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    int startScore = gameManager.fieldScore;
                    int rand = UnityEngine.Random.Range(0, 100);
                    float approachInf = (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    float rarityAdd = 1 + (cardInstance.rarity) / 45;
                    float odds = 30 / (approachInf * rarityAdd);
                    if (rand < odds)
                    {
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
            description = "Draw card cost = 0 \nCosts % of mana";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 10;
            rarity = 1;
            stars = 4;

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

    public class DuplicateCard : Card
    {
        public DuplicateCard()
        {
            cardName = "Duplicate Next Card";
            description = "Get a copy of the next card played into your hand";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 20;
            rarity = 1;
            stars = 4;

            CardCost = new ManaCardCost(
                (cardInstance) => {
                    float baseCost = GameManager.Instance.mana * 0.3f;
                    float rarityAdd = 1 + (cardInstance.rarity) / 5;
                    float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    if((baseCost * approachZero) - rarityAdd < 0)
                    {
                        return 0;
                    }
                    return (float)(baseCost * approachZero) - rarityAdd;
                }
            );

            CardReward = new DuplicateCardReward(
                (cardInstance) => {
                    return 0;
                }
            );
        }
    }

    public class ManaPauseCard : Card
    {
        public ManaPauseCard()
        {
            cardName = "Pause mana";
            description = "Pause mana decrease for time % \nCosts fixed coins";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 0;
            rarity = 1;
            stars = 3;

            CardCost = new ScoreCardCost(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    int startScore = gameManager.fieldScore;
                    float baseCost = 30f;
                    float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    return (float)(startScore * ((1f / 100f) * baseCost * approachZero));
                }
            );

            CardReward = new PauseManaCardReward(
                (cardInstance) => {
                    float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, 3);
                    float rarityAdd = 1 + (cardInstance.rarity / 50);
                    return ((100f - (80f * approachZero)) * rarityAdd)/10;
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



