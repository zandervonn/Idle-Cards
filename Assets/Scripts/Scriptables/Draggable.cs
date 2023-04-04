//4
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public bool playable = false;
    public GameObject cardDie;
    public CardDisplay cardDisplay;
    private DrawCard drawCard;
    private Card _cardComponent;

    public GameObject cardShadow;
    private Vector2 _lastPosition;
    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;
    private float _dragSpeed;
    private Vector3 previousPosition;
    private Quaternion currentTiltRotation;
    private Quaternion targetTiltRotation;

    private DeckManager deckManager;

    public Card CardComponent

    {
        get { return _cardComponent; }
        set { _cardComponent = value; }
    }

    private void Update()
    {
        // Smoothly interpolate the tilt rotation
        float smoothTime = 0.1f; // Adjust this value to control the smoothness of the tilt transition
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetTiltRotation, smoothTime);
    }

    void Awake() {
        cardDisplay = GetComponent<CardDisplay>();
        if (cardDisplay == null) {
            Debug.LogError("CardDisplay component not found on the same GameObject as Draggable.");
        }
        drawCard = FindObjectOfType<DrawCard>();
        if (drawCard == null) {
            Debug.LogError("DrawCard not found in the scene.");
        }
        cardShadow = transform.Find("cardShadow").gameObject;
        if (cardShadow == null)
        {
            Debug.LogError("CardShadow not found in the card.");
        }

        deckManager = FindObjectOfType<DeckManager>();
        if (deckManager == null)
        {
            Debug.LogError("DeckManager not found in the scene.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Check if the deck is open before allowing the card to be dragged
        if (deckManager.isDeckVisible){return;}
        parentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        cardDisplay = GetComponent<CardDisplay>();

        dragStartPosition = eventData.position;
        dragStartRotation = transform.localRotation;

        previousPosition = transform.position;

    }

    public void OnDrag(PointerEventData eventData)
    {
        // Check if the deck is open before allowing the card to be dragged
        if (deckManager.isDeckVisible){return;}
        Vector3 currentPosition = eventData.position;
        Vector3 direction = currentPosition - previousPosition;
        float speed = direction.magnitude / Time.deltaTime;

        float maxSpeed = 1000f; // Adjust this value to control how fast the card needs to be dragged to reach maximum tilt
        float tiltFactor = Mathf.Clamp01(speed / maxSpeed);
        float maxTiltAngle = 30f; // Adjust this value to control the maximum tilt angle

        // Calculate the target tilt angle
        float targetTiltAngle = tiltFactor * maxTiltAngle;

        // Calculate the tilt axis (limiting rotation to certain axes)
        Vector3 tiltAxis = Vector3.Cross(Vector3.forward, direction).normalized;
        tiltAxis.z = 0; // Prevent rotation around the Z-axis

        // Calculate the target tilt rotation
        targetTiltRotation = Quaternion.AngleAxis(targetTiltAngle, tiltAxis);

        // Update the card position
        this.transform.position = currentPosition;

        // Update the previous position for the next frame
        previousPosition = currentPosition;
    }



    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if the deck is open before allowing the card to be dragged
        if (deckManager.isDeckVisible){return;}
        if (playable)
        {
            // Get the Card component of the dragged object
            Card card = CardComponent;

            if (card != null)
            {
                card.OnDrop(GameManager.Instance, card);
            }
            else
            {
                Debug.Log("No card component found");
            }

            Instantiate(cardDie, this.transform.position, Quaternion.identity);

            // Call UpdateSpacing before destroying the game object
            DrawCard.Instance.OnCardDropped.Invoke();

            Destroy(this.gameObject);
        }
        else
        {
            this.transform.SetParent(parentToReturnTo);
            Debug.Log("Card not playable");
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        StartCoroutine(ResetTilt());
    }



    private IEnumerator ResetTilt()
    {
        float resetDuration = 2f;
        float elapsedTime = 0f;

        Quaternion startRotation = transform.localRotation;

        while (elapsedTime < resetDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / resetDuration;
            transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.identity, t);
            yield return null;
        }

        transform.localRotation = Quaternion.identity;
    }
}