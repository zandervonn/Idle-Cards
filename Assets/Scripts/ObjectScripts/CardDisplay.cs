//1
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CardDisplay : MonoBehaviour
{
    public Image cardImage;
    public Text cardName;
    public Text cardDescription;
    public Text cardLevel;
    public Text cardMana;
    public GameObject upgradePanel;
    private Card _card;
    public CardInstance cardInstance;

    public Card card
    {
        get
        {
            return _card;
        }
    }

    public void Setup(CardInstance cardInstance, bool showUpgradeButton)
    {
        this.cardInstance = cardInstance; // Add this line to set the cardInstance field
        Card card = cardInstance.card;

        cardName.text = card.cardName;
        cardDescription.text = card.description;
        cardImage.sprite = card.image;
        cardLevel.text = "" + cardInstance.level; //fix to build string

        Debug.Log("CardDisplay Setup - Card Name: " + card.cardName + ", Level: " + cardInstance.level); // Add this line

        upgradePanel.gameObject.SetActive(showUpgradeButton);
    }
}