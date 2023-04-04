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

    public void Setup(Card cardInstance, bool showUpgradeButton)
    {
        Card card = cardInstance;

        cardName.text = card.cardName;
        cardDescription.text = card.description;
        cardImage.sprite = card.image;
        cardLevel.text = "Lvl " + cardInstance.level;

        upgradePanel.gameObject.SetActive(showUpgradeButton);
    }
}