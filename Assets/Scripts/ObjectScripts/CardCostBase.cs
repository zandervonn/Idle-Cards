using System;
using UnityEngine;

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