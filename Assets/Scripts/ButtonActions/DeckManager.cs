//9
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckManager : MonoBehaviour, IPointerClickHandler
{
    public GameObject cardCollectionCanvas;
    public GameObject UpgradeCanvas;
    public Transform cardGrid;
    public bool isDeckVisible { get; private set; }
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }
        cardCollectionCanvas.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isDeckVisible = !isDeckVisible;
        cardCollectionCanvas.SetActive(isDeckVisible);

        if (isDeckVisible)
        {
            DisplayCards();
        }
    }

    public void DisplayCards()
    {
        // Clear any previous cards
        foreach (Transform child in cardGrid)
        {
            Destroy(child.gameObject);
        }

        // Display the cards
        foreach (CardInstance cardInstance in gameManager.cardManager.ownedCards)
        {
            CardDisplay cardDisplay = Instantiate(gameManager.cardsList.cardPrefab, cardGrid);

            // Set the sorting order of the instantiated card's canvas
            Canvas cardCanvas = cardDisplay.GetComponent<Canvas>();
            if (cardCanvas != null)
            {
                cardCanvas.overrideSorting = true;
                cardCanvas.sortingOrder = 1; // Set this value to be higher than the deck's canvas sorting order
            }
            else
            {
                Debug.LogError("No Canvas component found on the card.");
            }

            cardDisplay.Setup(cardInstance, true);
        }
    }
}
