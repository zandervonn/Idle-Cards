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
    private Button upgradeButton;
    public Text upgradeCardPrice;
    public float upgradeCardMultiplier;

    private void Start()
    {
        gameManager = GameManager.Instance;
        cardDisplay = GetComponentInParent<CardDisplay>();
        upgradeCardMultiplier = 1.8f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager gameManager = GameManager.Instance;
        int upgradeCost = cardDisplay.cardInstance.level;
        Debug.Log("upgrade card for $" + upgradeCost);
        if (gameManager.SpendBank(upgradeCost))
        {
            cardDisplay.cardInstance.Upgrade();
            upgradeCost += (int) (upgradeCost * upgradeCardMultiplier);
            upgradeCardPrice.text = "$" + upgradeCost;
        }
    }
}
