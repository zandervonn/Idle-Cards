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
        cards = new List<Card>();

        cards.Add(new BasicCard());
        cards.Add(new DoubleCard());
        cards.Add(new ManaResetCard());
        cards.Add(new DrawCardsCard());
        cards.Add(new ManaLeftCard());
    }

    //private Card CreateBasicCard()
    //{
    //    float cardCost_multi = 0.4f;
    //    float cardCost_fixed = 10f;

    //    Card basicCard = Card.CreateInstance(
    //        "The Basic",
    //        "Increase score by 1 x level",
    //        Resources.Load<Sprite>("CardImages/Coin"),
    //        null,
    //        startingLevel,
    //        baseCardManaCost,
    //        50
    //    );

    //    basicCard.cardRewardType = CardValueType.Score;
    //    basicCard.cardCostType = CardValueType.Mana;

    //    FormulaDelegate costFormula = (cardInstance) => {
    //        float baseCost = cardCost_fixed + (GameManager.Instance.mana * cardCost_multi);
    //        float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = 1/(1 + x/100)^speed
    //        return (float)(baseCost * approachZero);
    //    };
    //    basicCard.CostFormula = costFormula;

    //    FormulaDelegate rewardFormula = (cardInstance) => {
    //        //return 10f + (float)Math.Pow(1f + (cardInstance.level / 30f), 10); //f(x) = 1 + (1 + x/30)^10
    //        float rarityAdd = cardInstance.rarity / 10; //1-10 x
    //        return 10f + (cardInstance.level * (5f + rarityAdd)); //f(x) = x * 10, to keep the increase liniar

    //    };
    //    basicCard.RewardFormula = rewardFormula;

    //    basicCard.Actions = new List<Action<GameManager, CardInstance>>
    //    {
    //        (gameManager, cardInstance) => {
    //            float reward = rewardFormula(cardInstance);
    //            gameManager.IncreaseScore((int) reward);

    //            float cost = costFormula(cardInstance);
    //            gameManager.DecreaseMana((int) cost);
    //        }
    //    };

        
    //    basicCard.IsAffordable = (cardInstance, gameManager) => {
    //        return gameManager.mana >= costFormula(cardInstance);
    //    };

    //    return basicCard;
    //}

    //private Card CreateDoubleCard()
    //{

    //    float cardCost = 40f;

    //    Card doubleCard = Card.CreateInstance(
    //        "Multiply",
    //        "x your score \n half your mana",
    //        Resources.Load<Sprite>("CardImages/CoinBag"),
    //        null,
    //        startingLevel,
    //        baseCardManaCost,
    //        1
    //    );

    //    doubleCard.cardRewardType = CardValueType.Score;
    //    doubleCard.cardCostType = CardValueType.Mana;

    //    FormulaDelegate costFormula = (cardInstance) => {
    //        float baseCost = cardCost;
    //        float approachZero = (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = 1/(1 + x/100)^speed
    //        return (float)(baseCost * approachZero);
    //    };
    //    doubleCard.CostFormula = costFormula;

    //    FormulaDelegate rewardFormula = (cardInstance) => {
    //        GameManager gameManager = FindObjectOfType<GameManager>();
    //        int startScore = gameManager.fieldScore;
    //        float approachInf = (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = (1 + x/100)^speed
    //        float rarityAdd = 1 + (cardInstance.rarity) / 25; //1-5 x
    //        return startScore * approachInf * rarityAdd;
    //    };
    //    doubleCard.RewardFormula = rewardFormula;

    //    doubleCard.Actions = new List<Action<GameManager, CardInstance>>
    //    {
    //        (gameManager, cardInstance) => {
    //            float reward = rewardFormula(cardInstance); 
    //            gameManager.IncreaseScore((int) reward);

    //            float cost = costFormula(cardInstance);
    //            gameManager.DecreaseMana((int) cost);
    //        }
    //    };

    //    doubleCard.IsAffordable = (cardInstance, gameManager) => {
    //        return gameManager.mana >= costFormula(cardInstance);
    //    };

    //    return doubleCard;
    //}

    //private Card CreateManaResetCard()
    //{

    //    float cardCost = 30f;

    //    Card manaReset = Card.CreateInstance(
    //        "I NEED MORE TIME",
    //        null,
    //        Resources.Load<Sprite>("CardImages/Clock"),
    //        null,
    //        startingLevel,
    //        0,
    //        1
    //    );

    //    manaReset.cardRewardType = CardValueType.Mana;
    //    manaReset.cardCostType = CardValueType.Score;

    //    FormulaDelegate costFormula = (cardInstance) => {
    //        GameManager gameManager = FindObjectOfType<GameManager>();
    //        int startScore = gameManager.fieldScore;
    //        float baseCost = cardCost;
    //        float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = 1/(1 + x/100)^speed
            
    //        return (float)(startScore * ((1f / 100f) * baseCost * approachZero)); //costs x % of score
    //    };
    //    manaReset.CostFormula = costFormula;

    //    FormulaDelegate rewardFormula = (cardInstance) => {
    //        float approachZero = 1f / (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = 1/(1 + x/100)^speed
    //        float rarityAdd = 1 + (cardInstance.rarity / 50); // 1 - 3 x
    //        return (100f - (80f * approachZero)) * rarityAdd; //20 > 22 > ... > 100

    //    };
    //    manaReset.RewardFormula = rewardFormula;

    //    manaReset.Actions = new List<Action<GameManager, CardInstance>>
    //    {
    //        (gameManager, cardInstance) => {
    //            float reward = rewardFormula(cardInstance);
    //            gameManager.IncreaseMana((int) reward);

    //            float cost = costFormula(cardInstance);
    //            gameManager.DecreaseScore((int) cost);
    //        }
    //    };

    //    manaReset.IsAffordable = (cardInstance, gameManager) => {
    //        return gameManager.fieldScore >= costFormula(cardInstance);
    //    };

    //    return manaReset;
    //}

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //private Card CreateDrawCardsCard()
    //{
    //    float cardCost = 30f;

    //    Card drawCardsCard = Card.CreateInstance(
    //        "Draw Cards",
    //        null,
    //        Resources.Load<Sprite>("CardImages/Cards"),
    //        null,
    //        startingLevel,
    //        baseCardManaCost,
    //        1
    //    );

    //    drawCardsCard.cardRewardType = CardValueType.Card;
    //    drawCardsCard.cardCostType = CardValueType.Mana;

    //    FormulaDelegate costFormula = (cardInstance) => {
    //        float baseCost = cardCost;
    //        float approachZero = (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = 1/(1 + x/100)^speed
    //        return (float)(baseCost * approachZero);
    //    };
    //    drawCardsCard.CostFormula = costFormula;

    //    FormulaDelegate rewardFormula = (cardInstance) => {
    //        float rarityAdd = 2 + (cardInstance.rarity / 25); // 2 - 6
    //        return rarityAdd;

    //    };
    //    drawCardsCard.RewardFormula = rewardFormula;

    //    drawCardsCard.Actions = new List<Action<GameManager, CardInstance>>
    //    {
    //        (gameManager, cardInstance) => {
    //            float reward = rewardFormula(cardInstance);
    //            DrawCard drawCard = FindObjectOfType<DrawCard>();
    //            drawCard.DrawCards((int) reward);

    //            float cost = costFormula(cardInstance);
    //            gameManager.DecreaseMana((int) cost);
    //        }
    //    };

    //    drawCardsCard.IsAffordable = (cardInstance, gameManager) => {
    //        return gameManager.mana >= costFormula(cardInstance);
    //    };

    //    return drawCardsCard;
    //}

    //private Card CreateManaLeftCard()
    //{
    //    float cardCost = 10f;

    //    Card manaLeftCard = Card.CreateInstance(
    //        "Mana Left",
    //        "Multiply current mana, add it to score",
    //        null,
    //        null,
    //        startingLevel,
    //        baseCardManaCost,
    //        1
    //    );

    //    manaLeftCard.cardRewardType = CardValueType.Score;
    //    manaLeftCard.cardCostType = CardValueType.Mana;

    //    FormulaDelegate costFormula = (cardInstance) => {
    //        float baseCost = cardCost;
    //        float approachZero = (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = 1/(1 + x/100)^speed
    //        return (float)(baseCost * approachZero);
    //    };
    //    manaLeftCard.CostFormula = costFormula;

    //    FormulaDelegate rewardFormula = (cardInstance) => {
    //        GameManager gameManager = FindObjectOfType<GameManager>();
    //        float mana = gameManager.mana;
    //        float approachInf = (float)Math.Pow(1f + cardInstance.level / 100f, speedToZero); // f(x) = (1 + x/100)^speed
    //        float rarityAdd = 1 + (cardInstance.rarity) / 25; //1-5 x
    //        return (mana * approachInf * rarityAdd) / 10 ;
    //    };
    //    manaLeftCard.RewardFormula = rewardFormula;

    //    manaLeftCard.Actions = new List<Action<GameManager, CardInstance>>
    //    {
    //        (gameManager, cardInstance) => {
    //            float reward = rewardFormula(cardInstance);
    //            gameManager.IncreaseScore((int) reward);

    //            float cost = costFormula(cardInstance);
    //            gameManager.DecreaseMana((int) cost);
    //        }
    //    };

    //    manaLeftCard.IsAffordable = (cardInstance, gameManager) => {
    //        return gameManager.mana >= costFormula(cardInstance);
    //    };

    //    return manaLeftCard;
    //}

}



