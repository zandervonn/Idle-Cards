//3
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class DrawCard : MonoBehaviour
{
    public GameObject newParent;
    public float spacing = 0;
    public CardDisplay cardDisplay;
    private CardsList cardsList;
    private GameManager gameManager;
    public static DrawCard Instance;
    public UnityEvent OnCardDropped;

    private void Start()
    {
        DrawCards(5);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager gameManager = GameManager.Instance;
        if(gameManager.SpendRound(2)){
            DrawNewCard();
        }
    }

    public void DrawCards(int n)
    {
        for (int i = 0; i < n; i++)
        {
            DrawNewCard();
        }
    }

    public void DrawNewCard()
    {
        GameManager gameManager = GameManager.Instance;
        cardsList = gameManager.cardsList;

        // Check if there are any available cards to draw
        if (gameManager.cardManager.availableCards.Count == 0)
        {
            Debug.LogWarning("No cards available to draw.");
            return;
        }

        // Get a random card from the list of available cards
        int cardIndex = Random.Range(0, gameManager.cardManager.availableCards.Count);
        CardInstance cardInstance = gameManager.cardManager.availableCards[cardIndex];

        // Remove the drawn card from the available cards list
        gameManager.cardManager.availableCards.RemoveAt(cardIndex);

        // Instantiate a new CardDisplay instance
        CardDisplay newCardDisplay = Instantiate(cardDisplay, new Vector2(0, 0), Quaternion.identity);
        newCardDisplay.Setup(cardInstance, false);
        Debug.Log("Card drawn: " + cardInstance.card.cardName + ". Actions count: " + cardInstance.card.Actions.Count);

        // Set the card's parent and adjust spacing
        newCardDisplay.transform.SetParent(newParent.transform);

        // Set the card field of the Draggable component
        Draggable draggable = newCardDisplay.GetComponent<Draggable>();
        if (draggable != null)
        {
            draggable.CardComponent = cardInstance.card; // Set the CardComponent property in Draggable
            draggable.cardInstance = cardInstance; // Add this line
            Debug.Log("Card component and cardInstance set in Draggable");
        }

        UpdateSpacing();
    }

    public void UpdateSpacing() {
      int cardCount = newParent.transform.childCount;
      float handWidth = newParent.GetComponent<RectTransform>().rect.width - 100;
      float cardWidth = cardDisplay.GetComponent<RectTransform>().rect.width + 8f;
      if (cardWidth * cardCount > handWidth) {
          spacing = -((cardWidth * cardCount) - handWidth) / cardCount;
      }
      newParent.GetComponent<HorizontalLayoutGroup>().spacing = spacing;
  }

    public void ClearCards()
    {
        foreach (Transform child in newParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}