//11
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
        UpgradeCost = (int)(UpgradeCost * 1.5f * nextUpgradeExtra);
        float tp =  timesPlayed / 20; // usage wear, the more times plyed the more expensive to upgrade
        float rar = 1 - (rarity / 100);
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