//13
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RemoveCardButton : MonoBehaviour, IPointerDownHandler
{
    private GameManager gameManager;
    private CardDisplay cardDisplay;
    public Text removeCardPrice;

    private void Start()
    {
        gameManager = GameManager.Instance;
        cardDisplay = GetComponentInParent<CardDisplay>();
        UpdateRemoveCardPrice(); 
    }

    private void Update()
    {
        UpdateAllRemoveCardPrices();
    }



    public void OnPointerDown(PointerEventData eventData)
    {

        GameManager gameManager = GameManager.Instance;
        ModalDialog popup = ModalDialog.instance;
        int removeCost = gameManager.RemoveCost;
        popup.OpenDialog("Are you sure you want to remove this card for $" + removeCost + "?");

        popup.OnYes.AddListener(() =>
        {
            if (gameManager.SpendBank(gameManager.RemoveCost))
            {
                cardDisplay.cardInstance.Remove();

                // Update the remove card cost
                gameManager.UpdateRemoveCost();

                // Update remove card prices for all card instances
                UpdateAllRemoveCardPrices();

                // Update the deck display
                DeckManager deckManager = FindObjectOfType<DeckManager>();
                if (deckManager != null && deckManager.isDeckVisible)
                {
                    deckManager.DisplayCards();
                }
            }
        });

    }
    private void UpdateAllRemoveCardPrices()
    {
        RemoveCardButton[] removeCardButtons = FindObjectsOfType<RemoveCardButton>();
        foreach (RemoveCardButton button in removeCardButtons)
        {
            button.UpdateRemoveCardPrice();
        }
    }

    private void UpdateRemoveCardPrice()
    {
        GameManager gameManager = GameManager.Instance;
        int removeCost = gameManager.RemoveCost;

        removeCardPrice.text = "$" + removeCost;
    }
}