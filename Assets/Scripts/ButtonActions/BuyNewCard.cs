//15
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyNewCard : MonoBehaviour, IPointerDownHandler
{

    private CardsList cardsList;
    private SliderController sliderController;

    public Text cardCostText;
    private GameManager gameManager;

    private void Start()
    {
        cardsList = FindObjectOfType<CardsList>();
        gameManager = GameManager.Instance;
        sliderController = FindObjectOfType<SliderController>();
    }


    private void Update()
    {
        gameManager = GameManager.Instance;
        int cost = (int)(sliderController.costMultiplier * gameManager.BuyCost);
        cardCostText.text = "$" + cost;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BuyCard();
    }

    public void BuyCard()
    {
        cardsList = FindObjectOfType<CardsList>();
        
        ModalDialog popup = ModalDialog.instance;

        sliderController = FindObjectOfType<SliderController>();
        int cost = (int)(sliderController.costMultiplier * gameManager.BuyCost);

        popup.ClearListeners();

        if(cost > gameManager.BankValue)
        {
            popup.OpenOKDialog("You cannot afford to buy a new card");
        }
        else
        {
            popup.OpenYesNoDialog("Are you sure you want to buy a card for $" + cost + "?");
            popup.OnYes += () =>
            {
                if (gameManager.SpendBank(cost))
                {
                    // Get a random card from the list of card types (cardsList.cards)
                    int cardIndex = GetRandomCardIndexWithWeight();
                    Card card = cardsList.cards[cardIndex];

                    // Add the random card to the ownedCards list
                    SliderController sliderController = FindObjectOfType<SliderController>();
                    gameManager.cardManager.BuyNewOwnedCard(card, sliderController.GetLowerBound(), sliderController.GetUpperBound());
                    gameManager.UpdateBuyCost();

                    // Update the deck display
                    DeckManager deckManager = FindObjectOfType<DeckManager>();
                    if (deckManager != null && deckManager.isDeckVisible)
                    {
                        deckManager.DisplayCards();
                    }
                }
            };
        }
    }

    int GetRandomCardIndexWithWeight()
    {
        // Calculate total weight
        float totalWeight = 0;
        foreach (Card card in cardsList.cards)
        {
            totalWeight += 1.0f / card.stars;
        }

        // Generate a random number in the range [0, totalWeight)
        float randomWeight = Random.Range(0, totalWeight);

        // Determine which card this random weight corresponds to
        for (int cardIndex = 0; cardIndex < cardsList.cards.Count; ++cardIndex)
        {
            randomWeight -= 1.0f / cardsList.cards[cardIndex].stars;
            if (randomWeight <= 0)
            {
                return cardIndex;
            }
        }

        // This point should never be reached, but return the last card index as a fallback
        return cardsList.cards.Count - 1;
    }


}
