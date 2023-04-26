//3
using UnityEngine;
using UnityEngine.UI;
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

    //void Update()
    //{
    //    UpdateHand();
    //}

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
        float handWidth = newParent.GetComponent<RectTransform>().rect.width - (cardSpacing*2);
        float cardWidth = cardDisplay.GetComponent<RectTransform>().rect.width + cardSpacing;

        float totalCardWidth = cardWidth * cardCount;
        float availableSpacing = (totalCardWidth > handWidth) ? -((totalCardWidth - handWidth) / (cardCount - 1)) : (handWidth - totalCardWidth) / (cardCount - 1);

        // Calculate the shift required to center the hand
        //float shift = (handWidth - (cardWidth * cardCount + availableSpacing * (cardCount - 1))) / 2;
        float shift = handWidth / 4;

        for (int i = 0; i < cardCount; i++)
        {
            RectTransform cardTransform = newParent.transform.GetChild(i).GetComponent<RectTransform>();
            cardTransform.anchoredPosition = new Vector2(shift + (cardWidth + availableSpacing) * i, cardTransform.anchoredPosition.y);
        }
    }

    public void FanOutCards()
    {
        int cardCount = newParent.transform.childCount;
        float halfCardCount = Mathf.Floor(cardCount / 2);
        float maxTiltAngle = 10f;
        float maxHeight = 100f;

        if (cardCount <= 2) { return; }

        // Sort the cards by their local X position
        List<Transform> sortedCards = new List<Transform>();
        for (int i = 0; i < cardCount; i++)
        {
            sortedCards.Add(newParent.transform.GetChild(i));
        }
        sortedCards.Sort((a, b) => a.localPosition.x.CompareTo(b.localPosition.x));

        for (int i = 0; i < halfCardCount; i++)
        {
            float tiltAngle = (1 - (i/ halfCardCount)) * maxTiltAngle;
            float height = (((i / halfCardCount) * maxHeight) - (maxHeight/2));

            RectTransform cardTransform;

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
    }

    public void ClearCards()
    {
        foreach (Transform child in newParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}