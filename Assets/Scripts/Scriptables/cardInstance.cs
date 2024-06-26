//11
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardInstance
{
    public Card card;
    public int level;
    public int timesPlayed = 0;
    public float nextUpgradeExtra = 1;
    public int UpgradeCost { get; set; }
    private CardManager cardManager;
    public int rarity; 

    public CardInstance(Card card, CardManager cardManager)
    {
        if (card == null)
        {
            Debug.LogError("Card is null in CardInstance constructor.");
        }

        this.card = card;
        this.level = 0;
        UpgradeCost = 10;
        this.cardManager = cardManager;
        this.rarity = 1;
    }

    public CardInstance(Card card, CardManager cardManager, int level, int rarity)
    {
        if (card == null)
        {
            Debug.LogError("Card is null in CardInstance constructor. 3");
        }

        this.card = card;
        this.level = level;
        UpgradeCost = 10;
        this.cardManager = cardManager;
        this.rarity = rarity;
    }

    public void Upgrade()
    {
        UpgradeCost = (int)(UpgradeCost * 1.2f * nextUpgradeExtra); //1.5 > 1.2
        float tp =  timesPlayed / 40.0f; // usage wear, the more times plyed the more expensive to upgrade // 20 > 40
        float rar = 1 - (rarity / 100.0f); //the more rare, the cheaper playing it is
        nextUpgradeExtra = 1 + (tp * rar);

        Debug.Log("____________________________");
        Debug.Log("timesPlayed @ " + timesPlayed + " = " + tp.ToString("F2"));
        Debug.Log("rarity @ " + rarity);
        Debug.Log("nextUpgradeExtra @ " + nextUpgradeExtra + " = " + rar.ToString("F2"));


        timesPlayed = 0;
        level++;
    }

    public void Remove()
    {
        cardManager.RemoveCard(this);
    }
}