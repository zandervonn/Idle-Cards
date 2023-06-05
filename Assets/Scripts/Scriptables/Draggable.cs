//4
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public bool playable = false;
    public GameObject cardDie;
    public CardDisplay cardDisplay;
    private DrawCard drawCard;
    private Card _cardComponent;

    //public GameObject cardShadow;
    private Vector2 _lastPosition;
    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;
    private float _dragSpeed;
    private Vector3 previousPosition;
    private Quaternion currentTiltRotation;
    private Quaternion targetTiltRotation;
    private int originalIndex;

    private float scaleOnPickup = 1.7f;

    private DeckManager deckManager;
    public CardInstance cardInstance;

    private bool beingDragged = false;
    float smoothTime = 0.1f; // Adjust this value to control the smoothness of the tilt transition

    public Card CardComponent
    {
        get { return _cardComponent; }
        set { _cardComponent = value; }
    }

    private void Update()
    {
        if (beingDragged)
        {
            // Smoothly interpolate the tilt rotation
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetTiltRotation, smoothTime);
        }
    }

    void Awake()
    {
        cardDisplay = GetComponent<CardDisplay>();
        drawCard = FindObjectOfType<DrawCard>();

        deckManager = FindObjectOfType<DeckManager>();
        if (deckManager == null)
        {
            Debug.LogError("DeckManager not found in the scene.");
        }

        // Set CardComponent to the Card object of the CardInstance
        cardInstance = cardDisplay.cardInstance;
        CardComponent = cardInstance.card;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beingDragged = true;
        // Check if the deck is open before allowing the card to be dragged
        if (deckManager.isDeckVisible) { return; }
        originalIndex = this.transform.GetSiblingIndex();
        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        cardDisplay = GetComponent<CardDisplay>();

        // Scale the card up and set the pivot to the bottom-middle
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0.5f, 0f);
        rectTransform.localScale = new Vector3(scaleOnPickup, scaleOnPickup, 1f);


        dragStartPosition = eventData.position;
        dragStartRotation = transform.localRotation;

        previousPosition = transform.position;

        DrawCard.Instance.UpdateHand();

    }

    public void OnDrag(PointerEventData eventData)
    {
        beingDragged = true;
        // Check if the deck is open before allowing the card to be dragged
        if (deckManager.isDeckVisible) { return; }
        Vector3 currentPosition = eventData.position;
        Vector3 direction = currentPosition - previousPosition;
        float speed = direction.magnitude / Time.deltaTime;

        float velocityThreshold = 1f; // Adjust this value to control the minimum speed for tilt
        float maxSpeed = 80f; // Adjust this value to control how fast the card needs to be dragged to reach maximum tilt
        float maxTiltAngle = 20f; // Adjust this value to control the maximum tilt angle

        if (speed < velocityThreshold)
        {
            targetTiltRotation = Quaternion.AngleAxis(0, Vector3.zero);
        }
        else
        {
            float tiltFactor = Mathf.Clamp01(speed / maxSpeed);

            // Calculate the target tilt angle
            float targetTiltAngle = tiltFactor * maxTiltAngle;

            // Calculate the tilt axis (limiting rotation to certain axes)
            Vector3 tiltAxis = Vector3.Cross(Vector3.forward, direction).normalized;
            tiltAxis.z = 0; // Prevent rotation around the Z-axis

            // Calculate the target tilt rotation
            targetTiltRotation = Quaternion.AngleAxis(targetTiltAngle, tiltAxis);
        }

        // Update the card position
        this.transform.position = currentPosition;

        // Update the previous position for the next frame
        previousPosition = currentPosition;
    }


    public void OnEndDrag(PointerEventData eventData)
    {

        beingDragged = false;
        if (playable)
        {
            //float cardCost = CardComponent.CostFormula(cardInstance);
            if (CardComponent.IsAffordable(cardInstance, GameManager.Instance))
            {
                // Get the Card component of the dragged object
                Card card = CardComponent;
                Instantiate(cardDie, this.transform.position, Quaternion.identity);

                if (GameManager.Instance.DuplicateCard)
                {
                    card.OnDrop(GameManager.Instance, cardInstance);
                    this.transform.SetParent(parentToReturnTo);
                    this.transform.SetSiblingIndex(originalIndex);
                    GameManager.Instance.DuplicateCard = false;
                }
                else
                {
                    card.OnDrop(GameManager.Instance, cardInstance);

                    // Call UpdateSpacing before destroying the game object
                    DrawCard.Instance.OnCardDropped.Invoke();

                    Destroy(this.gameObject);
                }


            }
            else
            {
                this.transform.SetParent(parentToReturnTo);
                this.transform.SetSiblingIndex(originalIndex);
                Debug.Log("Card unaffordable");
            }
        }
        else
        {
            this.transform.SetParent(parentToReturnTo);
            this.transform.SetSiblingIndex(originalIndex);
            Debug.Log("Card not playable");
        }
        //}

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // Reset card size
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(1f, 1f, 1f);

        DrawCard.Instance.UpdateHand();
        targetTiltRotation = rectTransform.rotation;
    }

    public void CancelDragging()
    {
        beingDragged = false;

        // Reset card position and rotation
        transform.position = parentToReturnTo.position;
        transform.localRotation = dragStartRotation;

        // Reset card parent and blocksRaycasts
        transform.SetParent(parentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // Reset card scale
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }
}