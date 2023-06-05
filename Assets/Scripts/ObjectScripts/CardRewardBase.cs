using System;
using UnityEngine;

public interface ICardReward
{
    CardValueType RewardType { get; }
    float RewardFormula(CardInstance cardInstance);
    void ExecuteActions(GameManager gameManager, CardInstance cardInstance);
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
}

public class DrawCostCardReward : CardRewardBase
{
    public DrawCostCardReward(Func<CardInstance, float> rewardFormula) : base(rewardFormula) { }
    public override CardValueType RewardType => CardValueType.Bank;

    public override void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        gameManager.ResetDrawCost();
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
}

public class DuplicateCardReward : CardRewardBase
{
    public DuplicateCardReward(Func<CardInstance, float> rewardFormula) : base(rewardFormula) { }
    public override CardValueType RewardType => CardValueType.Card;

    public override void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        gameManager.DuplicateCard = true;
    }
}

public class PauseManaCardReward : CardRewardBase
{
    public PauseManaCardReward(Func<CardInstance, float> rewardFormula) : base(rewardFormula) { }
    public override CardValueType RewardType => CardValueType.Card;

    public override void ExecuteActions(GameManager gameManager, CardInstance cardInstance)
    {
        float reward = RewardFormula(cardInstance);
        gameManager.manaBarActions.PauseManaDecrease(reward);
    }
}