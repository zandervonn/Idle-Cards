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

    private void Start()
    {
        gameManager = GameManager.Instance;
        cardDisplay = GetComponentInParent<CardDisplay>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager gameManager = GameManager.Instance;
        int upgradeCost = 1 * cardDisplay.card.level;
        Debug.Log("upgrade card for $" + upgradeCost);
        if (gameManager.SpendBank(upgradeCost))
        {
            Upgrade();
        }
    }

    private void Upgrade()
    {
        Debug.Log("upgrade card clicked ");
        cardDisplay.card.Upgrade();
        UpdateUpgradeButton();
    }

    private void UpdateUpgradeButton()
    {
//        upgradeButton.GetComponentInChildren<Text>().text = "Upgrade (Level " + cardDisplay.card.level + ")";
    }
}
