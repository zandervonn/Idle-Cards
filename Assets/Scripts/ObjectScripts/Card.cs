//2
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using static UnityEditor.Progress;
using static CardsList;

public class Card { 
    public string cardName;
    public string description;
    public Sprite image;
    public List<Action<GameManager, CardInstance>> Actions;
    public int level;
    public int mana;
    public delegate float CardCostFormula(CardInstance cardInstance);
    public FormulaDelegate CostFormula { get; set; }
    public FormulaDelegate RewardFormula { get; set; }
    public delegate bool AffordableCheck(CardInstance cardInstance, GameManager gameManager);
    public AffordableCheck IsAffordable { get; set; }
    public CardValueType cardRewardType { get; set; }
    public CardValueType cardCostType { get; set; }
    public int rarity { get; set; } 


    public static Card CreateInstance(string cardName, string description, Sprite image, List<Action<GameManager, CardInstance>> actions, int initialLevel, int manaCost, int rarity)
    {
        Card card = new Card();

        card.cardName = cardName;
        card.description = description;
        card.image = image;
        card.Actions = actions ?? new List<Action<GameManager, CardInstance>>();
        card.level = initialLevel;
        card.mana = manaCost;
        card.rarity = rarity;

        return card;
    }
    
    public Card()
    {
        this.cardName = "";
        this.description ="";
        this.image = null;
        this.Actions = new List<Action<GameManager, CardInstance>>();
        this.level = 0;
        this.mana = 0;
    }

    public void OnDrop(GameManager gameManager, CardInstance cardInstance)
    {
        if (Actions != null)
        {
            foreach (var action in Actions)
            {
                if (action != null)
                {
                    action.Invoke(gameManager, cardInstance);
                }
            }
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
