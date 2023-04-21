//3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UpgradeCardButton : MonoBehaviour, IPointerDownHandler
{
    private GameManager gameManager;
    private CardDisplay cardDisplay;
    public Text upgradeCardPrice;

    private void Start()
    {
        gameManager = GameManager.Instance;
        cardDisplay = GetComponentInParent<CardDisplay>();
        UpdateUpgradeCardPrice();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager gameManager = GameManager.Instance;
        int upgradeCost = cardDisplay.cardInstance.UpgradeCost;
        if (gameManager.SpendBank(upgradeCost))
        {
            cardDisplay.cardInstance.Upgrade();
            UpdateUpgradeCardPrice();
        }

        // Update the deck display
        DeckManager deckManager = FindObjectOfType<DeckManager>();
        if (deckManager != null && deckManager.isDeckVisible)
        {
            deckManager.DisplayCards();
        }
    }

    private void UpdateUpgradeCardPrice()
    {
        int upgradeCost = cardDisplay.cardInstance.UpgradeCost;

        upgradeCardPrice.text = "$" + upgradeCost;
    }
}
