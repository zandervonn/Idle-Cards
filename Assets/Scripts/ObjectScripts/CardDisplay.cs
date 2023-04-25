﻿//1
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

    public Image costImage;
    public Image rewardImage;
    public Color scoreColor;
    public Color manaColor;
    public Color bankColor;

    public Image cardBorder;

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
        cardLevel.text = cardInstance.level.ToString("F0");

        upgradePanel.gameObject.SetActive(showUpgradeButton);

        UpdateBorderColor(cardInstance.rarity);
        UpdateValueColors();
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

    private void UpdateValueColors()
    {
        Card card = cardInstance.card;
        switch (card.cardCostType)
        {
            case CardValueType.Score:
                costImage.color = new Color(scoreColor.r, scoreColor.g, scoreColor.b, 1);
                break;
            case CardValueType.Mana:
                costImage.color = new Color(manaColor.r, manaColor.g, manaColor.b, 1);
                break;
            case CardValueType.Bank:
                costImage.color = new Color(bankColor.r, bankColor.g, bankColor.b, 1);
                break;
        }

        switch (card.cardRewardType)
        {
            case CardValueType.Score:
                rewardImage.color = new Color(scoreColor.r, scoreColor.g, scoreColor.b, 1);
                break;
            case CardValueType.Mana:
                rewardImage.color = new Color(manaColor.r, manaColor.g, manaColor.b, 1);
                break;
            case CardValueType.Bank:
                rewardImage.color = new Color(bankColor.r, bankColor.g, bankColor.b, 1);
                break;
        }
    }

    private void UpdateBorderColor(int rarity)
    {
        float rarityPercentage = (float)rarity / 100f;
        Color borderColor;

        float gray = 0.3f; //0-30
        float green = 0.5f; //30-50
        float blue = 0.8f; //50 - 80
        float magenta = 0.95f; //80-95
        float yellow = 1f; //95-100




        if (rarityPercentage < 0.3f)
        {
            borderColor = Color.Lerp(Color.gray, Color.green, rarityPercentage/gray);
        }
        else if (rarityPercentage < 0.5f)
        {
            borderColor = Color.Lerp(Color.green, Color.blue, (rarityPercentage-gray) / (green- gray));
        }
        else if (rarityPercentage < 0.80f)
        {
            borderColor = Color.Lerp(Color.blue, Color.magenta, (rarityPercentage - green) /(blue - green));
        }
        else if (rarityPercentage < 0.95f)
        {
            borderColor = Color.Lerp(Color.magenta, Color.yellow, (rarityPercentage - blue) / (magenta - blue));
        }
        else
        {
            borderColor = Color.yellow;
        }

        cardBorder.color = borderColor;
    }
}