//2
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using static UnityEditor.Progress;

public class Card { 
    public string cardName;
    public string description;
    public Sprite image;
    public List<Action<GameManager, CardInstance>> Actions;
    public int level;
    public int mana;
    public delegate float CardCostFormula(CardInstance cardInstance);
    public CardCostFormula CostFormula { get; set; }
    public delegate bool AffordableCheck(CardInstance cardInstance, GameManager gameManager);
    public AffordableCheck IsAffordable { get; set; }


    public static Card CreateInstance(string cardName, string description, Sprite image, List<Action<GameManager, CardInstance>> actions, int initialLevel, int manaCost)
    {
        Card card = new Card();

        card.cardName = cardName;
        card.description = description;
        card.image = image;
        card.Actions = actions ?? new List<Action<GameManager, CardInstance>>();
        card.level = initialLevel;
        card.mana = manaCost;

        Debug.Log("Card created. Actions count: " + card.Actions.Count);

        return card;
    }
    
    //new card setup
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
        Debug.Log("OnDrop called for card: " + cardName + " with Level: " + cardInstance.level); // Add this line
        //Debug.Log("OnDrop called for card: " + cardName);
        if (Actions != null)
        {
            Debug.Log("Actions count: " + Actions.Count);
            Debug.Log("running actions");
            foreach (var action in Actions)
            {
                if (action != null)
                {
                    Debug.Log("Running action: " + action.Method.Name);
                    action.Invoke(gameManager, cardInstance);
                }
                else
                {
                    Debug.Log("Action is null");
                }
            }
        }
    }
}