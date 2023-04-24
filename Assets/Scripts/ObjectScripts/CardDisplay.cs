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
    public Text cardMana;
    public GameObject upgradePanel;
    private Card _card;
    public CardInstance cardInstance;

    public Text cardLevel;
    public Text manaCostText;
    public Text gainValueText;

    public Card card
    {
        get
        {
            return _card;
        }
    }

    public void Setup(CardInstance cardInstance, bool showUpgradeButton)
    {
        this.cardInstance = cardInstance;
        Card card = cardInstance.card;

        cardName.text = card.cardName;
        cardDescription.text = card.description;
        cardImage.sprite = card.image;
        cardLevel.text = cardInstance.level.ToString("F2");

        upgradePanel.gameObject.SetActive(showUpgradeButton);
    }

    private void Update()
    {
        UpdateManaCost();
        UpdateGainValue();
    }

    private void UpdateManaCost()
    {
        Card card = cardInstance.card;
        float currentManaCost = card.CostFormula(cardInstance);
        manaCostText.text = currentManaCost.ToString();
    }

    private void UpdateGainValue()
    {
        Card card = cardInstance.card;
        float currentGainValue = card.RewardFormula(cardInstance);
        gainValueText.text = currentGainValue.ToString();
    }
}