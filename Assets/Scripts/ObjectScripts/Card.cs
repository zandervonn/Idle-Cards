//2
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
            CardReward.ExecuteActions(gameManager, cardInstance);
            CardCost.ExecuteActions(gameManager, cardInstance);
            cardInstance.timesPlayed++;
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