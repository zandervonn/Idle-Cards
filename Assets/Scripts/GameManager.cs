//7
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }
    public int fieldScore { get; private set; }
    private float _mana = 100;
    public ManaBarActions manaBarActions;
    public float mana {
        get { return _mana; }
        set {
            _mana = value;
        }
    }
    public int HighScore { get; set; }
    public int LastScore { get; set; }
    public int BankValue { get; set; }
    public int BuyCost { get; set; }
    public int RemoveCost { get; set; }
    public int RemoveCostMultiplier { get; set; }
    public int TotalMoneyEarned { get; set; }
    public float maxMana { get; set; }
    public float minMana = 10;
    public CardsList cardsList{ get; private set; }
    public DrawCard drawCard;
    public CardManager cardManager;
    public DrawCardButton drawCardButton;
    public SaveLoadManager saveLoadManager;


    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        manaBarActions = FindObjectOfType<ManaBarActions>();

        HighScore = 0;
        LastScore = 0;
        BankValue = 100; // testing
        BuyCost = 1;
        RemoveCost = 1;
        maxMana = 100;


        RemoveCostMultiplier = 2;
        UpdateRemoveCost();

        // Load the game state
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        // Initialize the cardsList variable
        cardsList = FindObjectOfType<CardsList>();

        // Check if CardsList is found and if not, log an error
        if (cardsList == null)
        {
            Debug.LogError("CardsList not found");
            return;
        }

        cardsList.Initialize();

        // Initialize the cardsList variable
        cardsList = FindObjectOfType<CardsList>();

        // Always initialize the CardManager
        cardManager = new CardManager(cardsList.cards);

        if (PlayerPrefs.HasKey("GameState"))
        {
            saveLoadManager.LoadGameState();
        }
        else
        {
            saveLoadManager.SaveGameState();
            OnResetRound();
        }

    }

    public void UpdateHighScore(int newScore)
    {
        if (newScore > HighScore)
        {
            HighScore = newScore;
            PlayerPrefs.SetInt("HighScore", HighScore);
        }
    }

    public void UpdateLastScore(int newScore)
    {
        LastScore = newScore;
    }

    public void UpdateBankValue(bool high, bool last)
    {
        int moneyEarned = 0;

        if (high)
        {
            moneyEarned += HighScore;
        }
        if (last)
        {
            moneyEarned += LastScore;
        }

        BankValue += moneyEarned;
        TotalMoneyEarned += moneyEarned;
        CalculateLevel();
    }

    public float CalculateLevel()
    {
        //return Mathf.Pow(2, Mathf.Log(TotalMoneyEarned, 5));
        return Mathf.Pow(2, Mathf.Log(TotalMoneyEarned) / Mathf.Log(5)) / 1000; ;
    }

    public void IncreaseScore(int amount) {
        fieldScore += amount;
        if(fieldScore < 0){
            fieldScore = 0;
        }
    }

    public void DecreaseScore(int amount) {
        if (fieldScore - amount > 0)
        {
            fieldScore -= amount;
        }
        else
        {
            fieldScore = 0;
        }
    }

    public void IncreaseMana(float amount) {
        if( mana + amount < maxMana)
        {
            mana += amount;
        }
        else
        {
            mana = maxMana;
        }
        
    }

    public void DecreaseMana(float amount) {
        if (mana - amount > 0)
        {
            mana -= amount;
        }
        else
        {
            mana = 0;
        }
    }

    public void IncreaseBank(int amount)
    {
        BankValue += amount;
    }

    public bool SpendBank(int cost) {
        if(BankValue >= cost){
            BankValue -= cost;
            return true;
        } else {
            Debug.Log("Cant afford this");
            return false;
        }
    }

    public bool SpendRound(int cost) {
        if(fieldScore >= cost){
            fieldScore -= cost;
            return true;
        } else {
            Debug.Log("Cant afford this");
            return false;
        }
    }


    public void OnManaDepleted() {
        updateScoreRecords();
        UpdateBankValue(true,true);
        ResetField();
    }

    public void OnResetRound() {
        updateScoreRecords();
        UpdateBankValue(false,true);
        ResetField();
    }

    public void ResetField()
    {
        CancelAllDraggingCards();
        DrawCard drawCardComponent = FindObjectOfType<DrawCard>();
        drawCardComponent.ClearCards();
        cardManager.ResetAvailableCards();
        drawCardComponent.DrawCards(3);
        manaBarActions.ResetElapsedTimeSinceRoundStart();
        ResetScore();
        ResetMana();
        ResetDrawCost();
    }

    public void ResetScore()
    {
        fieldScore = 1; //something to multiply
    }

    public void ResetDrawCost()
    {
        BuyCost =(int) (TotalMoneyEarned * 0.001f) + 1;
        DrawCardButton drawCardButton = FindObjectOfType<DrawCardButton>();
        drawCardButton.UpdateDrawPriceText();
    }

    public void updateScoreRecords()
    {
        UpdateHighScore(fieldScore);
        UpdateLastScore(fieldScore);
    }

    public void ResetMana()
    {
        mana = maxMana;
    }

    public void UpdateRemoveCost()
    {
        RemoveCost *= RemoveCostMultiplier;
    }

    public void ChangeMaxMana(float amount)
    {
        maxMana += amount;
        if (maxMana < 10)
        {
            maxMana = 10;
        }
        manaBarActions.UpdateSliderMaxValue();
    }

    public void DrawCards(int num)
    {
        drawCard = FindObjectOfType<DrawCard>();
        drawCard.DrawCards(num);
    }

    public void CancelAllDraggingCards()
    {
        Draggable[] draggableCards = FindObjectsOfType<Draggable>();
        foreach (Draggable draggableCard in draggableCards)
        {
            draggableCard.CancelDragging();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            saveLoadManager = FindObjectOfType<SaveLoadManager>();
            saveLoadManager.SaveGameState();
        }
    }

    private void OnApplicationQuit()
    {
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        saveLoadManager.SaveGameState();
    }
}