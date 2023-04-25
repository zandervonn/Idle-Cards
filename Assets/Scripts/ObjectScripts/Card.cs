//2
using System;
using UnityEngine;

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
