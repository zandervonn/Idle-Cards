using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardInstance
{
    public Card card;
    public int level;

    public CardInstance(Card card, int level)
    {
        this.card = card;
        this.level = level;
    }

    public void Upgrade()
    {
        level++;
    }
}