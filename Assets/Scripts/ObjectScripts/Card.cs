//2
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using static UnityEditor.Progress;
using static CardsList;


public abstract class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite artwork;
    public int startingLevel;
    public int baseCardManaCost;
    public int rarity;

    public ICardCost CardCost { get; set; }
    public ICardReward CardReward { get; set; }

    public bool IsAffordable(CardInstance cardInstance, GameManager gameManager)
    {
        return CardCost.IsAffordable(cardInstance, gameManager);
    }

    public float CostFormula(CardInstance cardInstance)
    {
        return CardCost.CostFormula(cardInstance);
    }

    public float RewardFormula(CardInstance cardInstance)
    {
        return CardReward.RewardFormula(cardInstance);
    }

    public void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        CardReward.ExecuteActions(gameManager, cardInstance);
    }

    public void OnDrop(GameManager gameManager, CardInstance cardInstance)
    {
        ExecuteActions(gameManager, cardInstance);
    }

}

public enum CardValueType
{
    Score,
    Bank,
    Mana,
    Time,
    Card
}




public interface ICardReward
{
    CardValueType RewardType { get; }
    float RewardFormula(CardInstance cardInstance);
    void ExecuteActions(GameManager gameManager, CardInstance cardInstance);

    Color GetColor();
}

public interface ICardCost
{
    CardValueType CostType { get; }
    float CostFormula(CardInstance cardInstance);
    bool IsAffordable(CardInstance cardInstance, GameManager gameManager);

    Color GetColor();
}

public class ManaCardCost : ICardCost
{
    public CardValueType CostType => CardValueType.Mana;
    private readonly Func<CardInstance, float> _costFormula;
    private readonly Action<GameManager, CardInstance> _executeActions;

    public ManaCardCost(Func<CardInstance, float> costFormula, Action<GameManager, CardInstance> executeActions = null)
    {
        _costFormula = costFormula;
        _executeActions = executeActions;
    }

    public float CostFormula(CardInstance cardInstance)
    {
        return _costFormula(cardInstance);
    }

    public bool IsAffordable(CardInstance cardInstance, GameManager gameManager)
    {
        return gameManager.mana >= CostFormula(cardInstance);
    }

    public void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        _executeActions?.Invoke(gameManager, cardInstance);
    }

    public Color GetColor()
    {
        return new Color(0, 1, 1, 1); // Replace with your manaColor
    }
}

public class ScoreCardReward : ICardReward
{
    public CardValueType RewardType => CardValueType.Score;
    private readonly Func<CardInstance, float> _rewardFormula;

    public ScoreCardReward(Func<CardInstance, float> rewardFormula)
    {
        _rewardFormula = rewardFormula;
    }

    public float RewardFormula(CardInstance cardInstance)
    {
        return _rewardFormula(cardInstance);
    }

    public void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        float reward = RewardFormula(cardInstance);
        gameManager.IncreaseScore((int)reward);
    }

    public Color GetColor()
    {
        return new Color(0, 1, 1, 1); // Replace with your manaColor
    }
}


public class ManaCardReward : ICardReward
{
    public CardValueType RewardType => CardValueType.Mana;
    private readonly Func<CardInstance, float> _rewardFormula;

    public ManaCardReward(Func<CardInstance, float> rewardFormula)
    {
        _rewardFormula = rewardFormula;
    }

    public float RewardFormula(CardInstance cardInstance)
    {
        return _rewardFormula(cardInstance);
    }

    public void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        float reward = RewardFormula(cardInstance);
        gameManager.IncreaseMana((int)reward);
    }

    public Color GetColor()
    {
        return new Color(0, 1, 1, 1); // Replace with your manaColor
    }
}

public class CardCardReward : ICardReward
{
    public CardValueType RewardType => CardValueType.Card;
    private readonly Func<CardInstance, float> _rewardFormula;

    public CardCardReward(Func<CardInstance, float> rewardFormula)
    {
        _rewardFormula = rewardFormula;
    }

    public float RewardFormula(CardInstance cardInstance)
    {
        return _rewardFormula(cardInstance);
    }

    public void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        float reward = RewardFormula(cardInstance);
        gameManager.DrawCards((int)reward);
    }

    public Color GetColor()
    {
        return new Color(0, 1, 1, 1); // Replace with your manaColor
    }
}

public class ScoreCardCost : ICardCost
{
    public CardValueType CostType => CardValueType.Score;
    private readonly Func<CardInstance, float> _costFormula;

    public ScoreCardCost(Func<CardInstance, float> costFormula)
    {
        _costFormula = costFormula;
    }

    public float CostFormula(CardInstance cardInstance)
    {
        return _costFormula(cardInstance);
    }

    public bool IsAffordable(CardInstance cardInstance, GameManager gameManager)
    {
        return gameManager.fieldScore >= CostFormula(cardInstance);
    }

    public Color GetColor()
    {
        return new Color(0, 1, 1, 1); // Replace with your manaColor
    }
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
                float baseCost = 10f;
                float approachZero = (float)Math.Pow(1f + cardInstance.level / 100f, 3);
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