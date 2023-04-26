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

        if (IsAffordable(cardInstance, gameManager))
        {
            CardCost.ExecuteActions(gameManager, cardInstance);
            CardReward.ExecuteActions(gameManager, cardInstance);
        }
        else
        {
            Debug.Log("Card not affordable");
        }
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






public interface ICardCost
{
    CardValueType CostType { get; }
    float CostFormula(CardInstance cardInstance);
    bool IsAffordable(CardInstance cardInstance, GameManager gameManager);
    void ExecuteActions(GameManager gameManager, CardInstance cardInstance);

    Color GetColor();
}

public abstract class CardCostBase : ICardCost
{
    public abstract CardValueType CostType { get; }
    protected readonly Func<CardInstance, float> _costFormula;

    protected CardCostBase(Func<CardInstance, float> costFormula)
    {
        _costFormula = costFormula;
    }

    public float CostFormula(CardInstance cardInstance)
    {
        return _costFormula(cardInstance);
    }

    public abstract bool IsAffordable(CardInstance cardInstance, GameManager gameManager);
    public abstract void ExecuteActions(GameManager gameManager, CardInstance cardInstance);
    public abstract Color GetColor();
}

public class ManaCardCost : CardCostBase
{
    public ManaCardCost(Func<CardInstance, float> costFormula) : base(costFormula) { }
    public override CardValueType CostType => CardValueType.Mana;

    public override bool IsAffordable(CardInstance cardInstance, GameManager gameManager)
    {
        return gameManager.mana >= CostFormula(cardInstance);
    }

    public override void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        
        float cost = CostFormula(cardInstance);

        Debug.Log("decreasing mana by: " + cost);
        gameManager.DecreaseMana((int)cost);
    }

    public override Color GetColor()
    {
        return new Color(0, 1, 1, 1); // Replace with your manaColor
    }
}


public class ScoreCardCost : CardCostBase
{
    public ScoreCardCost(Func<CardInstance, float> costFormula) : base(costFormula) { }
    public override CardValueType CostType => CardValueType.Score;

    public override bool IsAffordable(CardInstance cardInstance, GameManager gameManager)
    {
        return gameManager.fieldScore >= CostFormula(cardInstance);
    }
    public override void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        float cost = CostFormula(cardInstance);
        gameManager.DecreaseScore((int)cost);
    }

    public override Color GetColor()
    {
        return new Color(0, 1, 1, 1); // Replace with your manaColor
    }
}











public interface ICardReward
{
    CardValueType RewardType { get; }
    float RewardFormula(CardInstance cardInstance);
    void ExecuteActions(GameManager gameManager, CardInstance cardInstance);

    Color GetColor();
}

public abstract class CardRewardBase : ICardReward
{
    public abstract CardValueType RewardType { get; }
    protected readonly Func<CardInstance, float> _rewardFormula;

    protected CardRewardBase(Func<CardInstance, float> rewardFormula)
    {
        _rewardFormula = rewardFormula;
    }

    public float RewardFormula(CardInstance cardInstance)
    {
        return _rewardFormula(cardInstance);
    }

    public abstract void ExecuteActions(GameManager gameManager, CardInstance cardInstance);
    public abstract Color GetColor();
}

public class ScoreCardReward : CardRewardBase
{
    public ScoreCardReward(Func<CardInstance, float> rewardFormula) : base(rewardFormula) { }
    public override CardValueType RewardType => CardValueType.Score;

    public override void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        float reward = RewardFormula(cardInstance);
        gameManager.IncreaseScore((int)reward);
    }

    public override Color GetColor()
    {
        return new Color(0, 1, 1, 1); // Replace with your manaColor
    }
}

public class ManaCardReward : CardRewardBase
{
    public ManaCardReward(Func<CardInstance, float> rewardFormula) : base(rewardFormula) { }
    public override CardValueType RewardType => CardValueType.Mana;

    public override void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        float reward = RewardFormula(cardInstance);
        gameManager.IncreaseMana((int)reward);
    }

    public override Color GetColor()
    {
        return new Color(0, 1, 1, 1); // Replace with your manaColor
    }
}

public class CardCardReward : CardRewardBase
{
    public CardCardReward(Func<CardInstance, float> rewardFormula) : base(rewardFormula) { }
    public override CardValueType RewardType => CardValueType.Card;

    public override void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        float reward = RewardFormula(cardInstance);
        gameManager.DrawCards((int)reward);
    }

    public override Color GetColor()
    {
        return new Color(0, 1, 1, 1); // Replace with your manaColor
    }
}