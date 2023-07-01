//1
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public Image cardImage;
    public Text cardName;
    public Text cardDescription;
    public Text cardMana;
    public GameObject upgradePanel;
    public GameObject mainCardBits;
    public CardInstance cardInstance;


    public Text cardLevel;
    public Text manaCostText;
    public Text gainValueText;

    public Image costImage;
    public Image rewardImage;
    public Color scoreColor;
    public Color manaColor;
    public Color bankColor;
    public Color gold;

    public Image cardBorder;

    public Sprite scoreSprite;
    public Sprite manaSprite;
    public Sprite bankSprite;
    public Sprite timeSprite;
    public Sprite cardSprite;

    public void Setup(CardInstance cardInstance, bool showUpgradeButton)
    {
        this.cardInstance = cardInstance;
        Card card = cardInstance.card;

        if (card == null)
        {
            Debug.LogError("Card is null.");
            return;
        }


        cardName.text = card.cardName;
        cardDescription.text = card.descriptionFunc(cardInstance);
        cardLevel.text = cardInstance.level.ToString("F0");

        // Disable or enable Raycast Targets based on the visibility of the upgrade panel
        SetRaycastTargets(mainCardBits, !showUpgradeButton);

        upgradePanel.gameObject.SetActive(showUpgradeButton);

        UpdateBorderColor(cardInstance.rarity);
        UpdateValueColors();
        UpdateCostIcon();
        UpdateRewardIcon();
    }

    public void SetRaycastTargets(GameObject obj, bool value)
    {
        foreach (Transform child in obj.transform)
        {
            var canvasGroup = child.gameObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = child.gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.blocksRaycasts = value;

            if (child.childCount > 0)
            {
                SetRaycastTargets(child.gameObject, value);
            }
        }
    }

    private float updateInterval = 0.1f;
    private float updateTimer = 0f;

    private void Update()
    {

        updateTimer += Time.deltaTime;
        if (updateTimer >= updateInterval)
        {
            updateTimer = 0f;
            UpdateManaCost();
            UpdateGainValue();
        }
        if (updateTimer >= updateInterval*100)
        {
            UpdateDescription();
        }
    }

    private void UpdateManaCost()
    {
        Card card = cardInstance.card;
        float currentManaCost = card.CostFormula(cardInstance);
        manaCostText.text = $"{currentManaCost:F2}";
        if (!card.IsAffordable(cardInstance, GameManager.Instance))
        {
            manaCostText.color = Color.red;
        } else
        {
            manaCostText.color = gold;
        }
    }

    private void UpdateDescription()
    {
        Card card = cardInstance.card;
        cardDescription.text = card.descriptionFunc(cardInstance);
    }

    private void UpdateGainValue()
    {
        Card card = cardInstance.card;
        float currentGainValue = card.RewardFormula(cardInstance);
        gainValueText.text = $"{currentGainValue:F2}";
    }

    private void UpdateValueColors()
    {
        Card card = cardInstance.card;
        switch (card.CardCost.CostType)
        {
            case CardValueType.Score:
                costImage.color = new Color(scoreColor.r, scoreColor.g, scoreColor.b, 1);
                break;
            case CardValueType.Mana:
                costImage.color = new Color(manaColor.r, manaColor.g, manaColor.b, 1);
                break;
            case CardValueType.Bank:
                costImage.color = new Color(bankColor.r, bankColor.g, bankColor.b, 1);
                break;
        }

        switch (card.CardReward.RewardType)
        {
            case CardValueType.Score:
                rewardImage.color = new Color(scoreColor.r, scoreColor.g, scoreColor.b, 1);
                break;
            case CardValueType.Mana:
                rewardImage.color = new Color(manaColor.r, manaColor.g, manaColor.b, 1);
                break;
            case CardValueType.Bank:
                rewardImage.color = new Color(bankColor.r, bankColor.g, bankColor.b, 1);
                break;
        }
    }

    private void UpdateBorderColor(int rarity)
    {
        float rarityPercentage = (float)rarity / 100f;
        Color borderColor;

        if (rarityPercentage < 0.40f)
        {
            borderColor = Color.gray;
        }
        else if (rarityPercentage < 0.67f)
        {
            borderColor = Color.green;
        }
        else if (rarityPercentage < 0.87f)
        {
            borderColor = Color.blue;
        }
        else if (rarityPercentage < 0.95f)
        {
            borderColor = Color.magenta;
        }
        else
        {
            borderColor = Color.yellow;
        }

        cardBorder.color = borderColor;
    }

    private void UpdateCostIcon()
    {
        Card card = cardInstance.card;
        switch (card.CardCost.CostType)
        {
            case CardValueType.Score:
                costImage.sprite = scoreSprite;
                break;
            case CardValueType.Mana:
                costImage.sprite = manaSprite;
                break;
            case CardValueType.Bank:
                costImage.sprite = bankSprite;
                break;
            case CardValueType.Time:
                costImage.sprite = timeSprite;
                break;
            case CardValueType.Card:
                costImage.sprite = cardSprite;
                break;
        }
    }

    private void UpdateRewardIcon()
    {
        Card card = cardInstance.card;
        switch (card.CardReward.RewardType)
        {
            case CardValueType.Score:
                rewardImage.sprite = scoreSprite;
                break;
            case CardValueType.Mana:
                rewardImage.sprite = manaSprite;
                break;
            case CardValueType.Bank:
                rewardImage.sprite = bankSprite;
                break;
            case CardValueType.Time:
                rewardImage.sprite = timeSprite;
                break;
            case CardValueType.Card:
                rewardImage.sprite = cardSprite;
                break;
        }
    }
}