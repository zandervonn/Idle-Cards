//2
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;


[CreateAssetMenu(fileName = "Card", menuName = "Card Game/Card", order = 1)]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite image;
    public List<Action<GameManager>> Actions;
    public int level;
    public int mana;

    public static Card CreateInstance(string cardName, string description, Sprite image, List<Action<GameManager>> actions, int initialLevel, int manaCost)
    {
        Card card = ScriptableObject.CreateInstance<Card>();

        card.cardName = cardName;
        card.description = description;
        card.image = image;
        card.Actions = actions ?? new List<Action<GameManager>>();
        card.level = initialLevel;
        card.mana = manaCost;

        Debug.Log("Card created. Actions count: " + card.Actions.Count);

        return card;
    }

    public void OnDrop(GameManager gameManager)
    {
         Debug.Log("OnDrop called for card: " + cardName);
        if (Actions != null)
        {
            Debug.Log("Actions count: " + Actions.Count);
            Debug.Log("running actions");
            foreach (var action in Actions)
            {
                if (action != null)
                {
                    Debug.Log("Running action: " + action.Method.Name);
                    action.Invoke(gameManager);
                }
                else
                {
                    Debug.Log("Action is null");
                }
            }
        }
    }


    public void Upgrade()
    {
        level++;
    }
}