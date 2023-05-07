using System;
using UnityEngine;

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

public class BankCardReward : CardRewardBase
{
    public BankCardReward(Func<CardInstance, float> rewardFormula) : base(rewardFormula) { }
    public override CardValueType RewardType => CardValueType.Bank;

    public override void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        float reward = RewardFormula(cardInstance);
        gameManager.IncreaseBank((int)reward);
    }

    public override Color GetColor()
    {
        return new Color(0, 1, 1, 1); // Replace with your manaColor
    }
}

public class DrawCostCardReward : CardRewardBase
{
    public DrawCostCardReward(Func<CardInstance, float> rewardFormula) : base(rewardFormula) { }
    public override CardValueType RewardType => CardValueType.Bank;

    public override void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        gameManager.ResetDrawCost();
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