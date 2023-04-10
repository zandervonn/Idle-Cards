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

    public CardInstance(Card card, int level = 1)
    {
        this.card = card;
        this.level = level;
        UpgradeCost = 10;
    }

    public void Upgrade()
    {
        level++;
        UpgradeCost = (int)(UpgradeCost * 1.8f);
    }
}