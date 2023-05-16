//3
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections.Generic;

public class DrawCard : MonoBehaviour
{
    public GameObject newParent;
    public float spacing = 0;
    public CardDisplay cardDisplay;
    private CardsList cardsList;
    private GameManager gameManager;
    public static DrawCard Instance;
    public UnityEvent OnCardDropped;

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
        if (gameManager.SpendRound(2))
        {
            DrawNewCard();
        }
    }

    public void DrawCards(int n)
    {
        for (int i = 0; i < n; i++)
        {
            DrawNewCard();
        }

        int cardCount = newParent.transform.childCount;
        DrawCardButton drawCardButton = FindObjectOfType<DrawCardButton>();
        drawCardButton.UpdateDeckRemaining();
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

        // Set the card's parent and adjust spacing
        newCardDisplay.transform.SetParent(newParent.transform);

        // Set the card field of the Draggable component
        Draggable draggable = newCardDisplay.GetComponent<Draggable>();
        if (draggable != null)
        {
            draggable.CardComponent = cardInstance.card; // Set the CardComponent property in Draggable
            draggable.cardInstance = cardInstance; // Add this line
        }

        UpdateHand();
    }

    public void UpdateHand()
    {
        UpdateSpacing();
        FanOutCards();
    }

    public void UpdateSpacing()
    {
        int cardCount = newParent.transform.childCount;
        float cardSpacing = 50;
        float elementWidth = newParent.GetComponent<RectTransform>().rect.width;
        float handWidth = elementWidth - 2 * cardSpacing;
        float cardWidth = cardDisplay.GetComponent<RectTransform>().rect.width;
        float totalCardWidth = cardWidth * cardCount;
        float totalSpacing = cardSpacing * (cardCount - 1);
        float totalWidth = totalCardWidth + totalSpacing;

        for (int i = 0; i < cardCount; i++)
        {
            RectTransform cardTransform = newParent.transform.GetChild(i).GetComponent<RectTransform>();

            // Update positions for the cards
            cardTransform.pivot = new Vector2(0.5f, 0.5f);
            float spacing = handWidth / (cardCount + 1);
            float x = cardSpacing + (spacing * (i + 1));
            cardTransform.anchoredPosition = new Vector2(x, cardTransform.anchoredPosition.y);
        }
    }

    public void FanOutCards()
    {

        Debug.Log("fanning out cads");

        int cardCount = newParent.transform.childCount;
        Debug.Log("card count: " + cardCount);
        float halfCardCount = Mathf.Floor(cardCount / 2);
        float maxTiltAngle = 10f;
        float maxHeight = 100f;

        // Sort the cards by their local X position
        List<Transform> sortedCards = new List<Transform>();
        for (int i = 0; i < cardCount; i++)
        {
            sortedCards.Add(newParent.transform.GetChild(i));
        }
        sortedCards.Sort((a, b) => a.localPosition.x.CompareTo(b.localPosition.x));

        RectTransform cardTransform;

        for (int i = 0; i < halfCardCount; i++)
        {
            float tiltAngle = (1 - (i / halfCardCount)) * maxTiltAngle;
            float height = (((i / halfCardCount) * maxHeight) - (maxHeight / 2));

            // Apply transformations to the left half of the cards
            cardTransform = sortedCards[i].GetComponent<RectTransform>();
            cardTransform.pivot = new Vector2(0.5f, 0.5f);
            cardTransform.localEulerAngles = new Vector3(0f, 0f, tiltAngle);
            cardTransform.localPosition = new Vector3(cardTransform.localPosition.x, height, cardTransform.localPosition.z);

            // Apply transformations to the right half of the cards
            cardTransform = sortedCards[(cardCount - 1) - i].GetComponent<RectTransform>();
            cardTransform.pivot = new Vector2(0.5f, 0.5f);
            cardTransform.localEulerAngles = new Vector3(0f, 0f, -tiltAngle);
            cardTransform.localPosition = new Vector3(cardTransform.localPosition.x, height, cardTransform.localPosition.z);
        }

        if (cardCount % 2 == 1)
        { //if odd
            cardTransform = sortedCards[(int)System.Math.Floor((float)(cardCount / 2))].GetComponent<RectTransform>(); // middle element
            cardTransform.pivot = new Vector2(0.5f, 0.5f);
            cardTransform.localEulerAngles = new Vector3(0f, 0f, 0f);
            cardTransform.localPosition = new Vector3(cardTransform.localPosition.x, maxHeight / 2, cardTransform.localPosition.z);
        }

        for (int i = 0; i < cardCount; i++)
        {
            cardTransform = sortedCards[i].GetComponent<RectTransform>();
            cardTransform.localPosition = new Vector3(cardTransform.localPosition.x, cardTransform.localPosition.y, 90 - (i*10)); //150 - (i * (100/cardCount+1))

            Debug.Log("Setting rotation for card " + i + ": " + cardTransform.localEulerAngles);
        }


    }

    public void ClearCards()
    {
        List<Transform> childrenToDestroy = new List<Transform>();

        // First, collect all children you want to destroy
        foreach (Transform child in newParent.transform)
        {
            childrenToDestroy.Add(child);
        }

        // Then, destroy the collected children
        foreach (Transform child in childrenToDestroy)
        {
            DestroyImmediate(child.gameObject);
        }

        int cardCount = newParent.transform.childCount;
        Debug.Log("after clear cards: " + cardCount);
    }
}