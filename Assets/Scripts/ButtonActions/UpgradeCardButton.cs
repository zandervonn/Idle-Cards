//3
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        }else
        {
            ModalDialog popup = ModalDialog.instance;
            popup.ClearListeners();
            popup.OpenOKDialog("You cannot afford to upgrade this card");
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
