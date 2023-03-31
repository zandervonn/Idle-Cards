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

    public Card card
    {
        get
        {
            return _card;
        }
    }

    public void Setup(Card card, bool showUpgradeButton)
    {
        _card = card;

        if (cardImage != null && card.image != null)
            cardImage.sprite = card.image;

        if (cardName != null)
            cardName.text = card.cardName;

        if (cardDescription != null)
            cardDescription.text = card.description;

        if (cardLevel != null)
            cardLevel.text = card.level.ToString();

        if (cardMana != null)
            cardMana.text = card.mana.ToString();

        if (upgradePanel != null)
            upgradePanel.SetActive(showUpgradeButton);

        Debug.Log("Card display setup. name: " + card.cardName);
    }
}