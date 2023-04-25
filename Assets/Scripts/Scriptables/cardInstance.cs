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
    public int rarity; 

    public CardInstance(Card card, CardManager cardManager)
    {
        this.card = card;
        this.level = 0;
        UpgradeCost = 10;
        this.cardManager = cardManager;
        this.rarity = 1;
    }

    public CardInstance(Card card, CardManager cardManager, int level, int rarity)
    {
        this.card = card;
        this.level = level;
        UpgradeCost = 10;
        this.cardManager = cardManager;
        this.rarity = rarity;
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