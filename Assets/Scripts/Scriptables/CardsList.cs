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

            descriptionFunc = (cardInstance) => {
                float manaCost = BasicCostFormula(cardInstance.level, cardInstance.rarity, 100);
                float reward = BasicRewardFormula(cardInstance.level, cardInstance.rarity);
                string manaCostString = manaCost.ToString("F2");
                string rewardString = reward.ToString("F0");

                string color = "00C6D9";
                string coloredManaCost = $"<color={color}>{manaCostString}</color>";
                string coloredReward = $"<color={color}>{rewardString}</color>";

                return $"Add fixed {rewardString} coins\nCosts {manaCostString}% of mana";
            };

            CardCost = new ManaCardCost(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    return BasicCostFormula(cardInstance.level, cardInstance.rarity, gameManager.mana);
                }
            );

            CardReward = new ScoreCardReward(
                (cardInstance) => {
                    return BasicRewardFormula(cardInstance.level, cardInstance.rarity);
                }
            );
        }

        public static float BasicRewardFormula(int level, int rarity)
        {
            float rarityAdd = rarity / 20;
            return 4f + ((level * 0.4f) * (5f + rarityAdd)); // 1 > 0.4
        }

        public static float BasicCostFormula(int level, int rarity, float mana)
        {
            float baseCost = 10f + (mana * 0.4f);
            float approachZero = 1f / (float)Math.Pow(1f + (level + (rarity / 3)) / 100f, 3);
            return (float)(baseCost * approachZero);
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

            descriptionFunc = (cardInstance) => {
                float manaCost = MultiplyCostFormula(cardInstance.level);
                float reward = 1 + MultiplyRewardFormula(cardInstance.level, cardInstance.rarity, 1);
                string manaCostString = manaCost.ToString("F2");
                string rewardString = reward.ToString("F2");

                return $"Multiply your coins by {rewardString}x \nCosts fixed {manaCostString} mana";
            };

            CardCost = new ManaCardCost(
                (cardInstance) => {
                    return MultiplyCostFormula(cardInstance.level);
                }
            );

            CardReward = new ScoreCardReward(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    return MultiplyRewardFormula(cardInstance.level, cardInstance.rarity, gameManager.fieldScore);
                }
            );
        }

        public static float MultiplyRewardFormula(int level, int rarity, int score)
        {
            float approachInf = (float)Math.Pow(1f + (level / 130f), 2); //100 > 130 // 3>2
            float rarityAdd = 1 + (rarity / 80f); //100 > 60 > 80
            float val = 0.5f * (score * approachInf * rarityAdd);
            return val;
        }

        public static float MultiplyCostFormula(int level)
        {
            float baseCost = 40f;
            float approachZero = 1f / (float)Math.Pow(1f + level / 100f, 3);
            return (float)(baseCost * approachZero);
        }
    }

    public class ManaResetCard : Card
    {
        public ManaResetCard()
        {
            cardName = "Add Mana";
            description = "Increase mana by fixed amount \nCosts % of coins";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 0;
            rarity = 1;
            stars = 2;

            descriptionFunc = (cardInstance) => {
                float manaCost = ManaCostFormula(cardInstance.level, cardInstance.rarity, 100);
                float reward = ManaRewardFormula(cardInstance.level, cardInstance.rarity);
                string manaCostString = manaCost.ToString("F0");
                string rewardString = reward.ToString("F2");

                return $"Get {rewardString} mana\nCosts {manaCostString}% of coins";
            };

            CardCost = new ScoreCardCost(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    return ManaCostFormula(cardInstance.level, cardInstance.rarity, gameManager.fieldScore);
                }
            );

            CardReward = new ManaCardReward(
                (cardInstance) => {
                    return ManaRewardFormula(cardInstance.level, cardInstance.rarity);
                }
            );
        }
        public static float ManaRewardFormula(int level, int rarity)
        {
            float approachZero = 1f / (float)Math.Pow(1f + level / 100f, 3);
            float rarityAdd = 1 + (rarity / 50);
            return (50f - (40f * approachZero)) * rarityAdd;
        }

        public static float ManaCostFormula(int level, int rarity, int score)
        {
            float baseCost = 30f;
            float approachZeroLvl = 1f / (float)Math.Pow(1f + level / 100f, 3);
            float approachZeroRar = 1f / (float)Math.Pow(1f + rarity / 100f, 3);
            return (float)(score * baseCost * approachZeroRar * approachZeroLvl) / 100f;
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

            descriptionFunc = (cardInstance) => {
                float manaCost = DrawCardCostFormula(cardInstance.level);
                float reward = DrawCardRewardFormula(cardInstance.rarity);
                string manaCostString = manaCost.ToString("F2");
                string rewardString = reward.ToString("F0");

                return $"Draw {rewardString} cards\nCosts fixed {manaCostString} mana";
            };

            CardCost = new ManaCardCost(
                (cardInstance) => {
                    return DrawCardCostFormula(cardInstance.level);
                }
            );

            CardReward = new CardCardReward(
                (cardInstance) => {
                    return DrawCardRewardFormula(cardInstance.rarity);
                }
            );
        }
        public static float DrawCardRewardFormula(int rarity)
        {
            float rarityAdd = 2 + (rarity / 25);
            return rarityAdd;
        }

        public static float DrawCardCostFormula(int level)
        {
            float baseCost = 30f;
            float approachZero = (float)(1f / Math.Pow(1f + level / 100f, 3));
            return (float)(baseCost * approachZero);
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

            descriptionFunc = (cardInstance) => {
                float manaCost = ManaLeftCostFormula(cardInstance.level, 100);
                float reward = ManaLeftRewardFormula(cardInstance.level, cardInstance.rarity, GameManager.Instance.maxMana);
                string manaCostString = manaCost.ToString("F2");
                string rewardString = reward.ToString("F0");

                return $"Increase score exponential to mana, up to {rewardString} coins)\nCosts {manaCostString}% of mana";
            };

            CardCost = new ManaCardCost(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    return ManaLeftCostFormula(cardInstance.level, gameManager.mana);
                }
            );

            CardReward = new ScoreCardReward(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    return ManaLeftRewardFormula(cardInstance.level, cardInstance.rarity, gameManager.mana);
                }
            );
        }

        public static float ManaLeftRewardFormula(int level, int rarity, float mana)
        {
            float approachInf = (float)Math.Pow(1f + level / 100f, 3);
            float rarityAdd = 1 + (rarity) / 20;
            return (mana * approachInf * rarityAdd) / 8;
        }

        public static float ManaLeftCostFormula(int level, float mana)
        {
            mana = mana * 0.3f;
            float approachZero = 1f / (float)Math.Pow(1f + level / 100f, 3);
            return (float)(mana * approachZero);
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

            descriptionFunc = (cardInstance) => {
                float reward = PanicRewardFormula(cardInstance.level, cardInstance.rarity, 1);
                string rewardString = reward.ToString("F0");

                return $"The closer to 0 mana, the more score. Up to {rewardString}\nCosts 50% of mana";
            };

            CardCost = new ManaCardCost(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    return PanicCostFormula(gameManager.mana);
                }
            );

            CardReward = new ScoreCardReward(
                (cardInstance) =>
                {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    return PanicRewardFormula(cardInstance.level, cardInstance.rarity, gameManager.mana);
                }
            );
        }

        public static float PanicRewardFormula(int level, int rarity, float mana)
        {
            if (mana < 1) mana = 1;
            float approachInf = (float)Math.Pow(1f + level / 100f, 3);
            float rarityAdd = 1 + (rarity) / 33; //25 > 33
            return (float)(Math.Pow((10 / (mana + 1)), 2) * approachInf * rarityAdd) + 1;
        }

        public static float PanicCostFormula(float mana)
        {
            return mana * 0.5f;
        }
    }

    public class LottoCard : Card
    {
        public LottoCard()
        {
            cardName = "The Gambler";
            description = "High chance to increase coins by % \nLow chance to lose all coins";
            artwork = null;
            startingLevel = 1;
            baseCardManaCost = 10;
            rarity = 1;
            stars = 3;

            descriptionFunc = (cardInstance) => {
                float manaCost = LottoOddsFormula(cardInstance.level, cardInstance.rarity);
                float reward = LottoRewardFormula(cardInstance.level, 1);
                string manaCostString = manaCost.ToString("F2");
                string rewardString = reward.ToString("F2");

                return $"Increase score by {rewardString}x\nOr lose it all {manaCostString}% of the time";
            };

            CardCost = new ScoreCardCost(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    return LottoCostFormula(cardInstance.level, cardInstance.rarity, gameManager.fieldScore);
                }
            );

            CardReward = new ScoreCardReward(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    return LottoRewardFormula(cardInstance.level, gameManager.fieldScore);
                }
            );
        }

        public static int LottoCostFormula(int level, int rarity, int startScore)
        {
            int rand = UnityEngine.Random.Range(0, 100);
            float odds = LottoOddsFormula(level, rarity);
            if (rand < odds)
            {
                return startScore;
            }
            return 0;
        }

        public static float LottoOddsFormula(int level, int rarity)
        {
            float approachInf = (float)Math.Pow(1f + level / 100f, 3);
            float rarityAdd = 1 + (rarity) / 45;
            return 50 / (approachInf * rarityAdd); // 30 > 70 > 50
        }

        public static float LottoRewardFormula(int level, int startScore)
        {
            float approachInf = (float)Math.Pow(1f + level / 60f, 1.5f); // 2 > 1.5
            return startScore * approachInf;
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

            descriptionFunc = (cardInstance) => {
                float manaCost = DuplicateCostFormula(cardInstance.level, cardInstance.rarity, 100);
                string manaCostString = manaCost.ToString("F2");
                return $"Get a copy of the next card played into your hand. \nCosts {manaCostString}% of mana";
            };

            CardCost = new ManaCardCost(
                (cardInstance) => {
                    GameManager gameManager = FindObjectOfType<GameManager>();
                    return DuplicateCostFormula(cardInstance.level, cardInstance.rarity, gameManager.mana);
                }
            );

            CardReward = new DuplicateCardReward(
                (cardInstance) => {
                    return 0;
                }
            );
        }

        public static float DuplicateCostFormula(int level, int rarity, float mana)
        {
            float baseCost = 10 + (mana * 0.3f);
            float rarityAdd = 1 + (rarity) / 7; // 4 > 7
            float approachZero = 1f / (float)Math.Pow(1f + level / 100f, 3);
            if ((baseCost * approachZero) - rarityAdd < 0)
            {
                return 0;
            }
            return (float)(baseCost * approachZero) - rarityAdd;
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



