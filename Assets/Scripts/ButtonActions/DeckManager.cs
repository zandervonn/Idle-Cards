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

    private void DisplayCards()
    {
        // Clear any previous cards
        foreach (Transform child in cardGrid)
        {
            Destroy(child.gameObject);
        }

        // Display the cards
        foreach (Card card in gameManager.cardManager.ownedCards)
        {
            CardDisplay cardDisplay = Instantiate(gameManager.cardsList.cardPrefab, cardGrid);
            cardDisplay.Setup(card, true);
        }
    }
}
