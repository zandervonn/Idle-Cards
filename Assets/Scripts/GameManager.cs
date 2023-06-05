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
    public long BankValue { get; set; }
    public int BuyCost { get; set; }
    public float CurrentMultiplier { get; set; }
    public float LevelMultiplier { get; set; }
    public int RemoveCost { get; set; }
    public int ResetCost { get; set; }
    public int RemoveCostMultiplier { get; set; }
    public int TotalMoneyEarned { get; set; }
    public float ManaLossRate { get; set; }
    public float MaxManaChangeCost { get; set; }
    public float maxMana { get; set; }
    public float minMana = 10;
    public bool DuplicateCard = false;
    public DateTime LastPauseTime { get; set; }
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

        CurrentMultiplier = 0.1f;
        resetValues();

        RemoveCostMultiplier = 2;
        UpdateRemoveCost();

        // Load the game state
        saveLoadManager = FindObjectOfType<SaveLoadManager>();

        resetCards();

        // Always initialize the CardManager
        cardManager = new CardManager(cardsList.cards);
        

        if (PlayerPrefs.HasKey("GameState"))
        {
            saveLoadManager.LoadGameState();
        }
        else
        {
            saveLoadManager.SaveGameState();
        }
        DrawCard drawCardComponent = FindObjectOfType<DrawCard>();
        drawCardComponent.DrawCards(3);

    }
    public void resetValues()
    {
        HighScore = 0;
        LastScore = 0;
        BankValue = 1000000000; // testing
        BuyCost = 1;
        RemoveCost = 1;
        maxMana = 100;
        ManaLossRate = 2.5f;
        MaxManaChangeCost = 1.0f;
        TotalMoneyEarned = 0;
        ResetCost = 1;
    }

    public void resetCards()
    {
        // Initialize the cardsList variable
        cardsList = FindObjectOfType<CardsList>();

        // Check if CardsList is found and if not, log an error
        if (cardsList == null)
        {
            Debug.LogError("CardsList not found");
            return;
        }

        cardsList.Initialize();

        // Always initialize the CardManager
        cardManager = new CardManager(cardsList.cards);
    }

    public void UpdateHighScore(int newScore)
    {
        if (newScore > HighScore)
        {
            HighScore = newScore;
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
            moneyEarned += (int) (HighScore * CurrentMultiplier);
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

    private float lastDepletionTime;

    public void OnManaDepleted()
    {
        float currentTime = Time.time;
        if (lastDepletionTime > 0)
        {
            float depletionTime = currentTime - lastDepletionTime;
            Debug.Log("Real time to deplete mana: " + depletionTime);
        }
        lastDepletionTime = currentTime;

        updateScoreRecords();
        UpdateBankValue(true, true);
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
        if ( 1 < maxMana && maxMana < minMana)
        {
            maxMana = minMana;
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
            LastPauseTime = DateTime.UtcNow;
            saveLoadManager.SaveGameState();
        }
    }

    private void OnApplicationQuit()
    {
        LastPauseTime = DateTime.UtcNow;
        saveLoadManager.SaveGameState();
    }

    public void CalculateIdleEarnings()
    {
        // Calculate the time difference in seconds
        double timeDifference = (DateTime.UtcNow - LastPauseTime).TotalSeconds;

        // Calculate the time it takes to deplete mana
        float timeToDepleteMana = manaBarActions.CalculateTimeToDepleteMana();

        // Calculate how many earnings there were
        int numberOfEarnings = (int)(timeDifference / timeToDepleteMana);

        // Calculate the total idle earnings
        int totalIdleEarnings = (int) (numberOfEarnings * HighScore * CurrentMultiplier);

        // Add the idle earnings to the score
        IncreaseBank(totalIdleEarnings);

        // Print the time away
        Debug.Log("Time away: " + timeDifference + " seconds, Time per tick:" + timeToDepleteMana  + " Ticks: " + numberOfEarnings + ", idle earnings: " + totalIdleEarnings); //todo make popup
    }
    public void retire()
    {

        Debug.Log("retireing");
        float tmpMultiplier = CurrentMultiplier + LevelMultiplier;

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        resetValues();
        resetCards();
        ResetField();

        CurrentMultiplier = tmpMultiplier;
        saveLoadManager.SaveGameState();
    }

}