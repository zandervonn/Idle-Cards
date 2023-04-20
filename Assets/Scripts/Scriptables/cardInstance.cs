//11
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardInstance
{
    public Card card;
    public int level;
    public int UpgradeCost { get; private set; }
    private CardManager cardManager;

    public CardInstance(Card card, CardManager cardManager)
    {
        this.card = card;
        this.level = 1;
        UpgradeCost = 10;
        this.cardManager = cardManager;
    }

    public CardInstance(Card card, CardManager cardManager, int level)
    {
        this.card = card;
        this.level = level;
        UpgradeCost = 10;
        this.cardManager = cardManager;
    }

    public void Upgrade()
    {
        level++;
        UpgradeCost = (int)(UpgradeCost * 1.8f);
    }

    public void Remove()
    {
        cardManager.RemoveCard(this);
    }
}